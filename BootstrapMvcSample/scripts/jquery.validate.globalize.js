/// <reference path="jquery-1.8.3.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="globalize.js" />

/*!
* Monkey patch for jquery.validate.js to make use of Globalize.js number and date parsing
*/

$(document).ready(function () {

    var currentCulture = $("meta[name='accept-language']").prop("content");

    // Set Globalize to the current culture driven by the meta tag (if any)
    if (currentCulture) {
        Globalize.culture(currentCulture);
    }

    //Tell the validator that we want numbers parsed using Globalize.js
    $.validator.methods.number = function (value, element) {
        if (Globalize.parseFloat(value)) {
            return true;
        }
        return false;
    }

    //Tell the validator that we want dates parsed using Globalize.js
    $.validator.methods.date = function (value, element) {
        if (Globalize.parseDate(value)) {
            return true;
        }
        return false;
    }

    //Fix the range to use globalized methods
    jQuery.extend(jQuery.validator.methods, {
        range: function (value, element, param) {
            //Use the Globalization plugin to parse the value
            var val = Globalize.parseFloat(value);
            return this.optional(element) || (val >= param[0] && val <= param[1]);
        }
    });

});