define(['useraccountconfig', 'durandal/system', 'services/logger'],
    function (config, system, logger) {
    var imageSettings = config.imageSettings;
    var nulloDate = new Date(1900, 0, 1);
    var referenceCheckValidator;
    var Validator = breeze.Validator;

    var orderBy = {
        user: 'username'
    };

    var entityNames = {
        user: 'UserAccount'
    };

    var model = {
        applyUserValidators: applyUserValidators,
        configureMetadataStore: configureMetadataStore,
        createNullos: createNullos,
        entityNames: entityNames,
        orderBy: orderBy
    };

    return model;

    //#region Internal Methods
    function configureMetadataStore(metadataStore) {
        metadataStore.registerEntityTypeCtor(
            'UserAccount', function () { this.isPartial = false; }, userInitializer);

        referenceCheckValidator = createReferenceCheckValidator();
        Validator.register(referenceCheckValidator);
        log('Validators registered');
    }
    
    function createReferenceCheckValidator() {
        var name = 'realReferenceObject';
        var ctx = { messageTemplate: 'Missing %displayName%' };
        var val = new Validator(name, valFunction, ctx);
        log('Validators created');
        return val;
        
        function valFunction(value, context) {
            return value ? value.id() !== 0 : true;
        }
    }
        
    function applyUserValidators(metadataStore) {
        var types = ['user'];
        types.forEach(addValidator);
        log('Validators applied', types);
        
        function addValidator(propertyName) {
            var userType = metadataStore.getEntityType('UserAccount');
            userType.getProperty(propertyName)
                .validators.push(referenceCheckValidator);
        }
    }
    
    function createNullos(manager) {
        var unchanged = breeze.EntityState.Unchanged;

        createNullo(entityNames.user, { username: ' [Select a user]' });

        function createNullo(entityName, values) {
            var initialValues = values
                || { name: ' [Select a ' + entityName.toLowerCase() + ']' };
            return manager.createEntity(entityName, initialValues, unchanged);
        }

    }

    function userInitializer(user) {
        user.tagsFormatted = ko.computed({
            read: function () {
                var text = user.tags();
                return text ? text.replace(/\|/g, ', ') : text;
            },
            write: function (value) {
                user.tags(value.replace(/\, /g, '|'));
            }
        });
    }

    function makeImageName(source) {
        return imageSettings.imageBasePath +
            (source || imageSettings.unknownPersonImageSource);
    }

    function log(msg, data, showToast) {
        logger.log(msg, data, system.getModuleId(model), showToast);
    }
    //#endregion
});