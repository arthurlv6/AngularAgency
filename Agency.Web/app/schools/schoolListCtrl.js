(function () {
    "use strict";
    var app = angular.module("main");
    app.controller("schoolListCtrl",["schoolResource","schoolTypesResource", '$log', '$modal', schoolListCtrl]);
    function schoolListCtrl(schoolResource,schoolTypesResource, $log, $modal) {
            var vm = this;
        vm.schoolSearchInput = "";
        vm.loadData = function () {
            schoolResource.query(
            function (data) {
                schoolTypesResource.query(null, function (schoolTypeData) {
                    vm.schoolTypes = schoolTypeData;
                    console.log("get vm.schoolTypes", vm.schoolTypes);
                    vm.schools = data;
                });
            });
        };
        vm.loadData();
        vm.search = function() {
            schoolResource.query({ id: vm.searchInput },
                function(data) {
                    vm.schools = data;
                    console.log("search data return", data);
                    $log.userMessage("geting data from the remote server.");
                });
        };
        vm.edit = function (school) {
            //console.log(school);
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: "/app/schools/_schoolAddOrEdit.html",
                controller: 'schoolModalInstanceCtrl as vm',
                size: '',
                resolve: {
                    schoolItem: function () {
                        return school;
                    },
                    schoolTypes: function () {
                        return vm.schoolTypes;
                    }
                }
            });
        };
        vm.detail = function (id) {
            angular.forEach(vm.schools, function (school) {
                if (school.id === id) {
                    school.show = !school.show;
                }
            });
        }
        vm.add = function () {
            schoolResource.get({ id: 0 },
            function (data) {
                var modalInstance = $modal.open({
                    animation: true,
                    templateUrl: "/app/schools/_schoolAddOrEdit.html",
                    controller: 'schoolModalInstanceCtrl as vm',
                    size: '',
                    resolve: {
                        schoolItem: function () {
                            return data;
                        },
                        schoolTypes: function () {
                            return vm.schoolTypes;
                        }
                    }
                });
            });
            
        };
    }
    app.controller("schoolModalInstanceCtrl", ["schoolResource", "currentUser", "schoolItem", "schoolTypes", "$scope", "$modalInstance", modalInstanceCtrl]);
    function modalInstanceCtrl(schoolResource, currentUser, schoolItem,schoolTypes, $scope, $modalInstance) {
            var vm = this;
            vm.schoolTypes = schoolTypes;
            console.log("vm.schoolTypes", vm.schoolTypes);
            if (schoolItem.id === 0) {
                vm.title = "Add";
            } else {
                vm.title = "Edit";
            }
            vm.school = schoolItem;
            
            vm.ok = function (isValid) {
                if (isValid) {
                    if (vm.school && vm.school.id) {
                        vm.school.$update({ id: vm.school.id }, function (data) {
                            console.log("update return",data);
                            //$scope.loadData();
                            $modalInstance.dismiss();
                            //vm.message = "Update Successful";
                        });
                    } else {
                        vm.school.$save(function (data) {
                            //$scope.loadData();
                            console.log("save return",data);
                            $modalInstance.dismiss();
                            //vm.message = "Save Successful";
                        });
                    }
                } else {
                    alert("Please correct the validation errors first.");
                }
            };
            vm.cancel=function(){
                $modalInstance.dismiss('cancel');
            };
        }
}());
