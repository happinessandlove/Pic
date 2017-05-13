/**************************************************
 * by 丁浩
 * 2016-01-30
 **************************************************/
$.validator.methods.date = function (value, element)
{
    return this.optional(element) || isDate(value);
}
$.validator.unobtrusive.adapters.addBool("checked");
$.validator.unobtrusive.adapters.addBool("yearmonth");
$.validator.unobtrusive.adapters.addBool("datecn");
$.validator.unobtrusive.adapters.addBool("postalcode");
$.validator.unobtrusive.adapters.addBool("telephone");
$.validator.unobtrusive.adapters.addBool("mobile");
$.validator.unobtrusive.adapters.addBool("phone");
$.validator.unobtrusive.adapters.addBool("fax");
$.validator.unobtrusive.adapters.addBool("integer");
$.validator.unobtrusive.adapters.addBool("selectoption");
$.validator.unobtrusive.adapters.addBool("idcardno");
$.validator.unobtrusive.adapters.addBool("username");
$.validator.unobtrusive.adapters.addBool("pwd");
$.validator.unobtrusive.adapters.add('fixedlength', ["length"], function (options)
{
    var params = { length: parseInt(options.params.length) };
    options.rules['fixedlength'] = params;
    if (options.message) { options.messages['fixedlength'] = options.message; }
});
$.validator.unobtrusive.adapters.add("chslength", ["min", "max"], function (options)
{
    var params = { min: options.params.min, max: options.params.max };
    options.rules["chslength"] = params;
    if (options.message) { options.messages["chslength"] = options.message; }
});
$.validator.unobtrusive.adapters.add('compareto', ["original", "op", "type"], function (options)
{
    var params = { original: options.params.original, op: options.params.op, type: options.params.type };
    options.rules['compareto'] = params;
    if (options.message) { options.messages['compareto'] = options.message; }
});

$(function ()
{
    $.validator.addMethod("checked", function (value, element, param)
    {
        return $(element).prop("checked");
    });
    $.validator.addMethod("yearmonth", function (value, element, param)
    {
        if (!value) return true;
        if (/^\d{4}[\.\/-]\d{1,2}$/.test(value) == false) return false;
        var d = value.split(/[\.\/-]/);
        if (d[1] > 0 && d[1] < 13) return true;
        else return false;
    });
    $.validator.addMethod("datecn", function (value, element, param)
    {
        return this.optional(element) || isDate(value);
    });
    $.validator.addMethod("postalcode", function (value, element, param)
    {
        return this.optional(element) || /^[1-9]\d{5}$/.test(value);
    });
    $.validator.addMethod("telephone", function (value, element, param)
    {
        return this.optional(element) || /^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}(-\d{1,4}|\(\d{1,4}\))?$/.test(value);
    });
    $.validator.addMethod("mobile", function (value, element, param)
    {
        return this.optional(element) || /^1\d{10}$/.test(value);
    });
    $.validator.addMethod("phone", function (value, element, param)
    {
        return this.optional(element) || /(^1\d{10}$)|(^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}(-\d{1,4}|\(\d{1,4}\))?$)/.test(value);
    });
    $.validator.addMethod("fax", function (value, element, param)
    {
        return this.optional(element) || /^(\(0\d{2,3}\)|(0\d{2,3}-))?[2-9]\d{6,7}$/.test(value);
    });
    $.validator.addMethod("idcardno", function (value, element, param)
    {
        return this.optional(element) || isIdCardNo(value);
    });
    $.validator.addMethod("integer", function (value, element, param)
    {
        return this.optional(element) || /^([+-])?[0-9]*$/.test(value);
    });
    $.validator.addMethod("selectoption", function (value, element, param)
    {
        return this.optional(element) || (value != "0" && value != 0 && value != '');
    });
    $.validator.addMethod('fixedlength', function (value, element, param)
    {
        if (!value) return true;
        return value.length == param.length;
    });
    $.validator.addMethod('chslength', function (value, element, param)
    {
        if (!value) return true;
        var s = value.replace(/[^\x00-\xff]/g, "**");
        return (s.length >= param.min && s.length <= param.max);
    });
    $.validator.addMethod('username', function (value, element, param)
    {
        return this.optional(element) || /^[0-9a-zA-Z\u4e00-\u9fa5][@0-9a-zA-Z\._\-\u4e00-\u9fa5]*$/.test(value);
    });
    $.validator.addMethod('pwd', function (value, element, param)
    {
        if (!value) return true;
        if (!/^\w+$/.test(value)) return false;
        if (!/^(?![^a-zA-Z]+$)(?!\D+$).*$/.test(value) && !/^(?![^a-zA-Z]+$)(?![^_]+$).*$/.test(value) && !/^(?!\D+$)(?![^_]+$).*$/.test(value)) return false;
        return true;
    });
    $.validator.addMethod('compareto', function (value, element, param)
    {
        if (!value) return true;
        var value2 = $("#" + param.original).val();
        if (!value2) return true;
        var op = param.op;
        var type = param.type;
        switch (type)
        {
            case "Integer": case "Double": case "Currency": return compareNum(parseFloat(value), parseFloat(value2), op); break;
            case "String": return compareNum(value, value2, op); break;
            case "Date": return compareDate(value, value2, op); break;
            default: return true;
        }
        return false;
    });

});
function compareNum(value1, value2, op)
{
    switch (op)
    {
        case "==": return (value1 == value2); break;
        case ">": return (value1 > value2); break;
        case ">=": return (value1 >= value2); break;
        case "<": return (value1 < value2); break;
        case "<=": return (value1 <= value2); break;
        case "!=": return (value1 != value2); break;
        default: return true;
    }
}
function compareDate(value1, value2, op)
{
    var arrD1 = value1.split(" ");
    var arrD2 = value2.split(" ");
    var arrDate1 = arrD1[0].split(/[\.\/-]/);
    var arrDate2 = arrD2[0].split(/[\.\/-]/);
    var arrTime1 = arrD1[1] ? arrD1[1].split(":") : "00:00:000".split(":");
    var arrTime2 = arrD2[1] ? arrD2[1].split(":") : "00:00:000".split(":");
    var d1 = new Date(arrDate1[0], arrDate1[1], arrDate1[2], arrTime1[0], arrTime1[1], arrTime1[2], arrTime1[2]);
    var d2 = new Date(arrDate2[0], arrDate2[1], arrDate2[2], arrTime2[0], arrTime2[1], arrTime2[2], arrTime2[2]);
    return compareNum(Date.parse(d1), Date.parse(d2), op);
}

function isDate(date)
{
    if (/^\d{4}[\/\-]\d{1,2}[\/\-]\d{1,2}( \d{1,2}:\d{1,2}:\d{1,2})?$/.test(date) == false) return false;
    var d = date.split(/[\.\/-]/);
    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    if (((d[0] % 4 == 0) && (d[0] % 100 != 0)) || (d[0] % 400 == 0)) iaMonthDays[1] = 29;
    if (d[1] < 1 || d[1] > 12) return false;
    if (d[2] < 1 || d[2] > iaMonthDays[d[1] - 1]) return false;
    return true;
}

//验证身份证号
function isIdCardNo(num)
{
    var factorArr = new Array(7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1);
    var parityBit = new Array("1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2");
    var varArray = new Array();
    var intValue;
    var lngProduct = 0;
    var intCheckDigit;
    var intStrLen = num.length;
    var idNumber = num;
    // initialize
    if ((intStrLen != 15) && (intStrLen != 18)) return false;
    // check and set value
    for (i = 0; i < intStrLen; i++)
    {
        varArray[i] = idNumber.charAt(i);
        if ((varArray[i] < '0' || varArray[i] > '9') && (i != 17))
        {
            return false;
        }
        else if (i < 17)
        {
            varArray[i] = varArray[i] * factorArr[i];
        }
    }
    if (intStrLen == 18)
    {
        //check date
        var date8 = idNumber.substring(6, 14);
        if (isDate8(date8) == false) return false;
        // calculate the sum of the products
        for (i = 0; i < 17; i++)
        {
            lngProduct = lngProduct + varArray[i];
        }
        // calculate the check digit
        intCheckDigit = parityBit[lngProduct % 11];
        // check last digit
        if (varArray[17] != intCheckDigit) return false;
    }
    else
    {        //length is 15
        //check date
        var date6 = idNumber.substring(6, 12);
        if (isDate6(date6) == false) return false;
    }
    return true;

}
/**
* 判断是否为“YYMMDD”式的日期
*
*/
function isDate6(sDate)
{
    if (!/^[0-9]{6}$/.test(sDate)) return false;
    var year, month, day;
    year = "19" + sDate.substring(0, 2); //15位均认为年份前两位为19
    month = sDate.substring(2, 4);
    day = sDate.substring(4, 6);
    //        var currentYear = new Date().getFullYear();
    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    //        if (year < currentYear - 150 || year > currentYear - 10) return false;
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > iaMonthDays[month - 1]) return false;
    return true;
}
/**
* 判断是否为“YYYYMMDD”式的日期
*
*/
function isDate8(sDate)
{
    if (!/^[0-9]{8}$/.test(sDate)) return false;
    var year, month, day;
    year = sDate.substring(0, 4);
    month = sDate.substring(4, 6);
    day = sDate.substring(6, 8);
    //        var currentYear = new Date().getFullYear();
    var iaMonthDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    //        if (year < currentYear - 150 || year > currentYear - 10) return false;
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) iaMonthDays[1] = 29;
    if (month < 1 || month > 12) return false;
    if (day < 1 || day > iaMonthDays[month - 1]) return false;
    return true;
}