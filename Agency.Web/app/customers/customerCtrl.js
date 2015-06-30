(function () {
    "use strict";
    var app = angular.module("main");
    app.controller("customerCtrl", ["customerResource", '$log', '$modal', customerCtrl]);
    function customerCtrl(customerResource, $log, $modal) {
            var vm = this;
            vm.searchInput = "";
            vm.ready = false;
        customerResource.query(
            function (data) {
                vm.customers = data;
                vm.ready = true;
                console.log("data", data);
            });
        vm.search = function() {
            customerResource.query({ id: vm.searchInput },
                function(data) {
                    vm.customers = data;
                });
        };
        vm.edit = function (customer) {
            console.log("customer", customer);
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: "/app/customers/_customerAddOrEdit.html",
                controller: 'customerModalInstanceCtrl',
                size: '',
                resolve: {
                    item: function () {
                        return customer;
                    }
                }
            });
        };
        vm.detail = function (id) {
            angular.forEach(vm.customers, function (customer) {
                if (customer.id === id) {
                    customer.show = !customer.show;
                }
            });
        }
        vm.add = function () {
            customerResource.get({ id: 0 },
            function (data) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: "/app/customers/_customerAddOrEdit.html",
                    controller: 'customerModalInstanceCtrl',
                    size: '',
                    resolve: {
                        item: function () {
                            return data;
                        }
                    }
                });
            });
        };
        vm.showAuditDetail=function(auditId){
            angular.forEach(vm.customers, function (customer) {
                angular.forEach(customer.customerAudits, function (customerAudit) {
                    if (customerAudit.id === auditId) {
                        customerAudit.show = !customerAudit.show;
                    }
                })
            });
        }
    }
    app.controller("customerModalInstanceCtrl", ["customerResource", "currentUser", "item", "$scope", "$modalInstance", customerModalInstanceCtrl]);
    function customerModalInstanceCtrl(customerResource, currentUser, item, $scope, $modalInstance) {
            if (item.id === 0) {
                $scope.title = "Add";
            } else {
                $scope.title = "Edit";
            }
            $scope.customer = item;
            $scope.ok = function (isValid) {
                if (isValid) {
                    if ($scope.customer && $scope.customer.id) {
                        $scope.customer.$update({ id: $scope.customer.id }, function (data) {
                            $scope.message = "Update Successful";
                        });
                    } else {
                        $scope.customer.$save(function (data) {
                            $scope.message = "Save Successful";
                        });
                    }
                } else {
                    alert("Please correct the validation errors first.");
                }
            };
            $scope.cancel=function(){
                $modalInstance.dismiss('cancel');
            };
        }
}());
