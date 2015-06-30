(function () {
    "use strict";
    var app = angular.module("main",
        ["common.services",
            "ui.mask",
            "ui.bootstrap",
            "ui.router"]);
    

    app.config(["$stateProvider",
            "$urlRouterProvider",'$provide', '$logProvider',
            function ($stateProvider, $urlRouterProvider, $provide,$logProvider) {
                $logProvider.debugEnabled(true);
                $provide.decorator('$log', ['$delegate', logDecorator]);

                $urlRouterProvider.otherwise("/customers");
                $stateProvider
                    .state("home", {
                        url: "/",
                        templateUrl: "app/welcomeView.html"
                    })
                    // Products
                    .state("customers", {
                        url: "/customers",
                        templateUrl: "app/customers/customerView.html",
                        controller: "customerCtrl as vm"
                    })
                    .state("login", {
                        url: "/login",
                        templateUrl: "app/login.html",
                        controller: "ProductListCtrl as vm"
                    })
                    .state("productEdit", {
                        abstract: true,
                        url: "/products/edit/:id",
                        templateUrl: "app/products/productEditView.html",
                        controller: "ProductEditCtrl as vm",
                        resolve: {
                            productResource: "productResource",
                            product: function (productResource, $stateParams) {
                                var pid = $stateParams.id;
                                return productResource.get({ id: pid }).$promise;
                            }
                        }
                    })
                    .state("productEdit.info", {
                        url: "/info",
                        templateUrl: "app/products/productEditInfoView.html"
                    })
                    .state("productEdit.price", {
                        url: "/price",
                        templateUrl: "app/products/productEditPriceView.html"
                    })
                    .state("productDetail", {
                        url: "/products/:productId",
                        templateUrl: "app/products/productDetailView.html",
                        controller: "ProductDetailCtrl as vm",
                        resolve: {
                            productResource: "productResource",

                            product: function(productResource, $stateParams) {
                                var productId = $stateParams.productId;
                                return productResource.get({ id: productId }).$promise;
                            }
                        }
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