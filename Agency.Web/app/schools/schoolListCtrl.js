(function () {
    "use strict";
    var app = angular.module("main");
        app.controller("schoolListCtrl",["schoolResource", '$log', '$modal', schoolListCtrl]);
        function schoolListCtrl(schoolResource, $log, $modal) {
            var vm = this;
        vm.schoolSearchInput = "";
        schoolResource.query({
                $orderby: "CreateDate",
                $top:10
            },
            function (data) {
                vm.schools = data;
                console.log("data", data);
            });
        vm.search = function() {
            schoolResource.query({
                $filter: "contains(Name, '" + vm.schoolSearchInput + "')",
                $orderby: "CreateDate",
                $top:10
                },
                function(data) {
                    vm.schools = data;
                    $log.userMessage("geting data from the remote server.");
                });
        };
        vm.edit = function (school) {
            //console.log(school);
            var modalInstance = $modal.open({
                animation: true,
                templateUrl: "/app/schools/_addOrEdit.html",
                controller: 'modalInstanceCtrl',
                size: '',
                resolve: {
                    schoolItem: function () {
                        return school;
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
                    templateUrl: "/app/schools/_addOrEdit.html",
                    controller: 'modalInstanceCtrl',
                    size: '',
                    resolve: {
                        schoolItem: function () {
                            return data;
                        }
                    }
                });
            });
            
        };
    }
        app.controller("modalInstanceCtrl", ["schoolResource", "currentUser", "schoolItem", "$scope", "$modalInstance", modalInstanceCtrl]);
        function modalInstanceCtrl(schoolResource, currentUser, schoolItem, $scope, $modalInstance) {
            if (schoolItem.id === 0) {
                $scope.title = "Add";
            } else {
                $scope.title = "Edit";
            }
            $scope.school = schoolItem;
            console.log("schoolItem", schoolItem);
            $scope.ok = function (isValid) {
                if (isValid) {
                    if ($scope.school && $scope.school.id) {
                        $scope.school.$update({ id: $scope.school.id }, function (data) {
                            $scope.message = "Update Successful";
                        });
                    } else {
                        $scope.school.$save(function (data) {
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
