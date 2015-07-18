(function () {
    "use strict";
    var app = angular.module("main");
    app.controller("schoolListCtrl",["schoolResource","schoolTypesResource", '$log', '$modal', schoolListCtrl]);
    function schoolListCtrl(schoolResource,schoolTypesResource, $log, $modal) {
            var vm = this;
            vm.searchInput = "";
            vm.pageNumber = 1;
            vm.totalCount = 0;
            vm.maxSize = 5;
        vm.loadData = function () {
            schoolResource.query({ search: '', pageNumber :1},
            function (data) {
                schoolTypesResource.query(null, function (schoolTypeData) {
                    vm.schoolTypes = schoolTypeData;
                    //console.log("DATA", data);
                    vm.schools = data;
                });
            });
            schoolResource.get({ condition: '' },
            function (data) {
                vm.totalCount = data.count;
                console.log("totalCount", data.count);
            });
        };
        vm.loadData();
        vm.search = function() {
            schoolResource.query({ search: vm.searchInput, pageNumber: vm.pageNumber },
                function(data) {
                    schoolTypesResource.query(null, function (schoolTypeData) {
                        vm.schoolTypes = schoolTypeData;
                        //console.log("get vm.schoolTypes", vm.schoolTypes);
                        vm.schools = data;
                        console.log("data", data);
                    });
                });
            schoolResource.get({ condition: vm.searchInput },
            function (data) {
                vm.totalCount = data.count;
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
        vm.orderBy = "id";
        vm.setOrderBy = function (item) {
            if (vm.orderBy === item) {
                vm.orderBy = "-"+item;
            } else {
                vm.orderBy = item;
            }

            
        }
        //pagination
        vm.pageChanged = function () {
            vm.search();
            console.log('Page changed to: '+vm.pageNumber);
        };
    }
    app.controller("schoolModalInstanceCtrl", ["schoolResource", "currentUser", "schoolItem", "schoolTypes", "$scope", "$modalInstance", "$filter", modalInstanceCtrl]);
    function modalInstanceCtrl(schoolResource, currentUser, schoolItem, schoolTypes, $scope, $modalInstance, $filter) {
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
                            vm.schoolTypes.forEach(function (item) {
                                if (item.id == data.schoolTypeId) {
                                    vm.school.schoolType.name = item.name;
                                }
                            });
                            $modalInstance.dismiss();
                        });
                    } else {
                        vm.school.$save(function (data) {
                            vm.schoolTypes.forEach(function (item) {
                                if (item.id == data.schoolTypeId) {
                                    vm.school.schoolType.name = item.name;
                                }
                            });
                            $modalInstance.dismiss();
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
