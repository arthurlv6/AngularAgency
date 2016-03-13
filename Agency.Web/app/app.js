(function () {
    "use strict";
    var app = angular.module("main",
        ["common.services", "ui.mask", "ui.bootstrap", "ui.router", "ngCookies"]);
    
    app.config(["$stateProvider","$urlRouterProvider",'$provide', '$logProvider',
            function ($stateProvider, $urlRouterProvider, $provide,$logProvider) {
                $logProvider.debugEnabled(true);
                $provide.decorator('$log', ['$delegate', logDecorator]);
                $urlRouterProvider.otherwise("home");
                $stateProvider
                    .state("home", {
                        url: "/home",
                        templateUrl: "app/home/welcomeView.html",
                        controller: "welcomeCtrl as vm"
                    })
                    .state("customers", {
                        url: "/customers",
                        templateUrl: "app/customers/customerView.html",
                        controller: "customerCtrl as vm"
                    })
                    .state("setting", {
                        url: "/setting",
                        templateUrl: "app/settings/settingMainCtrl.html",
                        controller: "settingMainCtrl as vm"
                    })
                    .state("settingCompany", {
                        url: "/settingCompany",
                        templateUrl: "app/settings/_company.html",
                        controller: "settingCompanyCtrl as vm"
                    })
                .state("schoolList", {
                    url: "/schools",
                    templateUrl: "app/schools/schoolListView.html",
                    controller: "schoolListCtrl as vm"
                });
            }]
    );
    function logDecorator($delegate) {

        function log(message) {
            message += ' - ' + new Date();
            $delegate.log(message);
        }

        function info(message) {
            $delegate.info(message);
        }

        function warn(message) {
            $delegate.warn(message);
        }

        function error(message) {
            $delegate.error(message);
        }

        function debug(message) {
            $delegate.debug(message);
        }

        function userMessage(message) {
            message = 'customized - ' + message+' '+new Date();
            $delegate.debug(message);
        }

        return {
            log: log,
            info: info,
            warn: warn,
            error: error,
            debug: debug,
            userMessage: userMessage
        };

    }
}());