define([
    'durandal/system',
    'services/UserAccount/model',
    'useraccountconfig',
    'services/logger',
    'services/breeze.partial-entities'],
    function (system, model, config, logger, partialMapper) {
        var EntityQuery = breeze.EntityQuery;
        var manager = configureBreezeManager();
        var orderBy = model.orderBy;
        var entityNames = model.entityNames;
        
        var getUserPartials = function (usersObservable, forceRemote) {
            if (!forceRemote) {
                var s = getLocal('Users', orderBy.user);
                if (s.length > 3) {
                    // Edge case
                    // We need this check because we may have 1 entity already.
                    // If we start on a specific person, this may happen. So we check for > 2, really
                    usersObservable(s);
                    return Q.resolve();
                }
            }

            var query = EntityQuery.from('Users')
                .select('ID, Username, Email, LastLogin, IsAccountVerified')
                .orderBy('Username');

            return manager.executeQuery(query)
                .then(querySucceeded)
                .fail(queryFailed);

            function querySucceeded(data) {
                var list = partialMapper.mapDtosToEntities(
                    manager, data.results, entityNames.user, 'id');
                if (usersObservable) {
                    usersObservable(list);
                }
                log('Retrieved [Users] from remote data source',
                    data, true);
            }
        };

        var getUserById = function(userId, userObservable) {
            // 1st - fetchEntityByKey will look in local cache 
            // first (because 3rd parm is true) 
            // if not there then it will go remote
            return manager.fetchEntityByKey(
                entityNames.user, userId, true)
                .then(fetchSucceeded)
                .fail(queryFailed);
            
            // 2nd - Refresh the entity from remote store (if needed)
            function fetchSucceeded(data) {
                var s = data.entity;
                return s.isPartial() ? refreshUser(s) : userObservable(s);
            }
            
            function refreshUser(user) {
                return EntityQuery.fromEntities(user)
                    .using(manager).execute()
                    .then(querySucceeded)
                    .fail(queryFailed);
            }
            
            function querySucceeded(data) {
                var s = data.results[0];
                s.isPartial(false);
                log('Retrieved [User] from remote data source', s, true);
                return userObservable(s);
            }

        };

        var cancelChanges = function() {
            manager.rejectChanges();
            log('Canceled login', null, true);
        };

        var saveChanges = function() {
            return manager.saveChanges()
                .then(saveSucceeded)
                .fail(saveFailed);
            
            function saveSucceeded(saveResult) {
                log('Signed In successfully', saveResult, true);
            }
            
            function saveFailed(error) {
                var msg = 'Sign In failed: ' + getErrorMessages(error);
                logError(msg, error);
                error.message = msg;
                throw error;
            }
        };

        var primeData = function () {
            var promise = Q.all([getUserPartials(null, true)])
                .then(applyValidators);

            return promise.then(success);
            
            function success() {
                 log('Primed data', datacontext.lookups);
            }
            
            function applyValidators() {
                model.applyUserValidators(manager.metadataStore);
            }

        };

        var createUser = function() {
            return manager.createEntity(entityNames.user);
        };

        var hasChanges = ko.observable(false);

        manager.hasChangesChanged.subscribe(function(eventArgs) {
            hasChanges(eventArgs.hasChanges);
        });

        var datacontext = {
            createUser: createUser,
            getUserPartials: getUserPartials,
            hasChanges: hasChanges,
            getUserById: getUserById,
            primeData: primeData,
            cancelChanges: cancelChanges,
            saveChanges: saveChanges
        };

        return datacontext;

        //#region Internal methods        
        
        function getLocal(resource, ordering, includeNullos) {
            var query = EntityQuery.from(resource)
                .orderBy(ordering);
            if (!includeNullos) {
                query = query.where('id', '!=', 0);
            }
            return manager.executeQueryLocally(query);
        }
        
        function getErrorMessages(error) {
            var msg = error.message;
            if (msg.match(/validation error/i)) {
                return getValidationMessages(error);
            }
            return msg;
        }
        
        function getValidationMessages(error) {
            try {
                //foreach entity with a validation error
                return error.entitiesWithErrors.map(function(entity) {
                    // get each validation error
                    return entity.entityAspect.getValidationErrors().map(function(valError) {
                        // return the error message from the validation
                        return valError.errorMessage;
                    }).join('; <br/>');
                }).join('; <br/>');
            }
            catch (e) { }
            return 'validation error';
        }

        function queryFailed(error) {
            var msg = 'Error retreiving data. ' + error.message;
            logError(msg, error);
            throw error;
        }

        function configureBreezeManager() {
            breeze.NamingConvention.camelCase.setAsDefault();
            var mgr = new breeze.EntityManager(config.remoteServiceName);
            model.configureMetadataStore(mgr.metadataStore);
            return mgr;
        }

        function getLookups() {
            return EntityQuery.from('Lookups')
                .using(manager).execute()
                .then(processLookups)
                .fail(queryFailed);
        }
        
        function processLookups() {
            model.createNullos(manager);
        }


        function log(msg, data, showToast) {
            logger.log(msg, data, system.getModuleId(datacontext), showToast);
        }

        function logError(msg, error) {
            logger.logError(msg, error, system.getModuleId(datacontext), true);
        }
        //#endregion
});