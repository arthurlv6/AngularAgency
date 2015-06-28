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
            //console.log(school);
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: "/app/customers/_addOrEdit.html",
                controller: 'modalInstanceCtrl',
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
                    templateUrl: "/app/customers/_addOrEdit.html",
                    controller: 'modalInstanceCtrl',
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
        app.controller("modalInstanceCtrl", ["customerResource", "currentUser", "item", "$scope", "$modalInstance", modalInstanceCtrl]);
        function modalInstanceCtrl(customerResource, currentUser, item, $scope, $modalInstance) {
            if (item.id === 0) {
                $scope.title = "Add";
            } else {
                $scope.title = "Edit";
            }
            $scope.item = item;
            console.log("item", item);
            $scope.ok = function (isValid) {
                if (isValid) {
                    if ($scope.item && $scope.item.id) {
                        $scope.item.$update({ id: $scope.item.id }, function (data) {
                            $scope.message = "Update Successful";
                        });
                    } else {
                        $scope.item.$save(function (data) {
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
