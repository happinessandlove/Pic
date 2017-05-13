/**************************************************
 * v1.1
 * by 丁浩
 * 2014-11-18
 **************************************************/
$(function ()
{
    $.fn.extend(
    {
        spinner:
            function (param)
            {
                var settings =
                    {
                        step: 1
                        , min: 0
                        , max: 65536
                    	, canFocus: true
                    	, onblur: null
                    	, invalidMessage: '数值超出范围！'
                    	, outOfRangeEvent: null
                    	, changeValueEvent: null
                    };
                $(this).each(function ()
                {
                    var input = $(this);
                    var attrs =
						{
						    step: input.attr('step')
							, min: input.attr('min')
							, max: input.attr('max')
                    	    , canFocus: input.attr('canFocus')
                    	    , onblur: input.attr('onblurx')
                    	    , invalidMessage: input.attr('invalidMessage')
                    	    , outOfRangeEvent: input.attr('outOfRangeEvent')
                    	    , changeValueEvent: input.attr('changeValueEvent')
						};
                    var o = $.extend(true, {}, settings, attrs, param);
                    o.min = parseFloat(o.min);
                    o.max - parseFloat(o.max);
                    o.step = parseFloat(o.step);
                    if (o.changeValueEvent != null && typeof o.changeValueEvent == "string")
                        o.changeValueEvent = eval(o.changeValueEvent);
                    if (o.outOfRangeEvent != null && typeof o.outOfRangeEvent == "string")
                        o.outOfRangeEvent = eval(o.outOfRangeEvent);

                    var sub = $('<span class="spinner-sub"></span>');
                    var add = $('<span class="spinner-add"></span>');
                    input.wrap('<span class="spinner-input"></span>');
                    var spinnerInput=input.parent();
                    spinnerInput.wrap('<span class="spinner-container"></span>');
                    var spinner = spinnerInput.parent();
                    spinner.append(sub).append(add);

                    var precision = 0;
                    if ((o.step + '').indexOf('.') > 0) precision = (o.step + '').substr((o.step + '').indexOf('.') + 1).length;
                    input.data('data', parseFloat(input.val()));
                    sub.on('click', function ()
                    {
                        if (sub.hasClass('disable')) return false;
                        var val = input.val();
                        if (val === '') val = o.step;
                        val = parseFloat(val) - o.step;
                        if (!checkValue(val)) return;
                        var oldVal = parseFloat(input.data('data'));
                        input.val(val).data('data', val);
                        if (o.changeValueEvent != null) o.changeValueEvent(input, val, oldVal);
                        return false;
                    });
                    add.on('click', function ()
                    {
                        if (add.hasClass('disable')) return false;
                        var val = input.val();
                        if (val === '') val = o.step;
                        val = parseFloat(val) + o.step;
                        if (!checkValue(val)) return;
                        var oldVal = parseFloat(input.data('data'));
                        input.val(val).data('data', val);
                        if (o.changeValueEvent != null) o.changeValueEvent(input, val, oldVal);
                        return false;
                    });
                    if (o.canFocus)
                    {
                        input.on('blur', function ()
                        {
                            var val = parseFloat(input.val());
                            var oldVal = parseFloat(input.data('data'));
                            if (!checkValue(val))
                            {
                                input.val(oldVal);
                                checkValue(oldVal);
                                return;
                            }
                            input.data('data', val);
                            if (o.onblur != null) o.onblur();
                            if (o.changeValueEvent != null) o.changeValueEvent(input, val, oldVal);
                            return false;
                        });
                        input.on('focus', function ()
                        {
                            $(this).select();
                            return false;
                        });
                    }
                    function checkValue(v)
                    {
                        var flag = true;
                        if (v === '') flag = false;
                        else if (!/^-?\d+(.?\d+)?$/.test(v)) flag = false;
                        else if (!/^\d+$/.test(v * Math.pow(10, precision) / o.step * Math.pow(10, precision))) flag = false;
                        if (v == o.min) sub.addClass('disable');
                        else sub.removeClass('disable');
                        if (v == o.max) add.addClass('disable');
                        else add.removeClass('disable');
                        if (v > o.max || v < o.min)
                        {
                            flag = false; sub.addClass('disable'); add.addClass('disable');
                        }
                        if (!flag)
                            if (o.outOfRangeEvent != null) o.outOfRangeEvent(input, o.min, o.max);
                            else alert(o.invalidMessage.replace('{0}', o.min).replace('{1}', o.max));
                        return flag;
                    }
                    function parseVal(v)
                    {
                        if (precision > 0) v = v.toFixed(precision);
                        return v;
                    }
                    checkValue(parseFloat(input.val()));
                });
            }
    })
});