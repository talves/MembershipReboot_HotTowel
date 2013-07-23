define(['services/logger'], function (logger) {
    var vm = {
        activate: activate,
        title: 'Login View',
        description: 'This page will have a form inside for sign-in.'
};

    return vm;

    //#region Internal Methods
    function activate() {
        logger.log('Login View Activated', null, 'login', true);
        return true;
    }
    //#endregion
});