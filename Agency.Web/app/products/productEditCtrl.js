

(function () {
    "use strict";

    angular
        .module("main")
        .controller("ProductEditCtrl",
        ["product",
            "$state", "$log",
            ProductEditCtrl]);


    function ProductEditCtrl(product, $state, $log) {
        var vm = this;
        vm.product = product;
        vm.message = "";
        $log.userMessage("test");
        if (vm.product && vm.product.id) {
            vm.title = "Edit: " + vm.product.name;
        }
        else {
            vm.title = "New Product";
        }

        vm.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            vm.opened = !vm.opened;
        };

        vm.submit = function (isValid) {
            if (isValid) {
                if (vm.product && vm.product.id) {
                    vm.product.$update({id:vm.product.id},function (data) {
                        vm.message = "Update Successful";
                    });
                } else {
                    vm.product.$save(function (data) {
                        vm.message = "Save Successful";
                    });
                }
            } else {
                alert("Please correct the validation errors first.");
            }
        };

        vm.back = function () {
            $state.go('productList');
        };

       
    }
}());

