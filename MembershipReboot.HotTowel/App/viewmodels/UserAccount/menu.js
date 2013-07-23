define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: 'Menu View'
    };

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Menu View Activated', null, 'menu', true);
        return true;
    }
    //#endregion
});