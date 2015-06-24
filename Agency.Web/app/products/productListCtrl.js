(function () {
    "use strict";
    angular
        .module("main")
        .controller("ProductListCtrl",
                     ["productResource",'$log',
                         ProductListCtrl]);

    function ProductListCtrl(productResource,$log) {
        var vm = this;
        vm.productSearchInput = "";
        productResource.query({
                //$filter: "contains(ProductCode, 'A') and Price ge 0 and Price le 2000",
                $orderby: "Id desc"
            },
            function(data) {
                vm.products = data;
            });
        vm.search = function() {
            productResource.query({

                    $filter: "contains(Name, '" + vm.productSearchInput + "')" +
                        " or contains(ProductCode, '" + vm.productSearchInput + "')",
                    $orderby: "Name"
                },
                function(data) {
                    vm.products = data;
                    $log.userMessage("geting data from the remote server.");
                });
        };
    }
}());
