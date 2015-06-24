(function () {
    "use strict";
    var app = angular.module("main");

        app.controller("schoolListCtrl",
                     ["schoolResource", '$log','$modal',
                         schoolListCtrl]);
        function schoolListCtrl(schoolResource, $log, $modal) {
        var vm = this;
        vm.schoolSearchInput = "";
        schoolResource.query({
                //$filter: "contains(Name, 'A') and Fee ge 1000 and Fee le 2000",
                $orderby: "Id desc"
            },
            function (data) {
                console.log(data);
                vm.schools = data;
            });
        vm.search = function() {
            schoolResource.query({
                $filter: "contains(Name, '" + vm.schoolSearchInput + "')",
                    $orderby: "Name"
                },
                function(data) {
                    vm.schools = data;
                    $log.userMessage("geting data from the remote server.");
                });
        };
        vm.edit = function (school) {
            console.log(school);
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
    }
        app.controller("modalInstanceCtrl", ["schoolResource", "currentUser","schoolItem","$scope", modalInstanceCtrl]);
        function modalInstanceCtrl(schoolResource, currentUser,schoolItem,$scope) {
            $scope.school=schoolItem;
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

            };
        }
}());
