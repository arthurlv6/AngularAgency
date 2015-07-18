(function () {
    "use strict";
    var app = angular.module("main");
    app.controller("settingCompanyCtrl", [settingCompanyCtrl]);
    function settingCompanyCtrl() {
        var vm = this;
        vm.radioModel = 'Company';
        vm.selected = function () {
            console.log("what", vm.radioModel);
        };
    }
}());
