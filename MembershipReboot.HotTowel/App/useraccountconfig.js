define(function () {
    toastr.options.timeOut = 4000;
    toastr.options.positionClass = 'toast-bottom-right';

    var imageSettings = {
        imageBasePath: '../content/images/photos/',
        unknownPersonImageSource: 'unknown_person.jpg'
    };

    var remoteServiceName = 'Account';

    var appTitle = 'MRHT';

    var routes = [{
        url: 'UserAccount/login',
        moduleId: 'viewmodels/UserAccount/login',
        name: 'Login',
        visible: true,
        caption: 'Login',
        settings: { caption: '<i class="icon-book"></i> Login' }
        },
        {
        url: 'UserAccount/menu',
        moduleId: 'viewmodels/UserAccount/menu',
        name: 'Menu',
        visible: true,
        caption: 'Menu',
        settings: { caption: '<i class="icon-book"></i> Login' }
        }];
    
    var startModule = 'viewmodels/UserAccount/login';

    return {
        appTitle: appTitle,
        debugEnabled: ko.observable(true),
        imageSettings: imageSettings,
        remoteServiceName: remoteServiceName,
        routes: routes,
        startModule: startModule
    };
});