/**************************************************
 * v1.2
 * by 丁浩
 * 2015-03-19
 **************************************************/
$(function ()
{
    $.fn.extend(
    {
        /// id：遮罩层的id。
        /// color：遮罩层的背景色。默认为'#eeeeee'
        /// opacity：遮罩层的透明度。默认为0.8
        /// duration：动画时间。默认为300毫秒 
        /// zIndex：层次。默认为500
        /// adaptiveSize：遮罩具体元素是，指定是否在窗体改变大小时调整遮罩层的大小
        mask: function (param)
        {
            var config = { id: null, color: '#eeeeee', opacity: 0.8, duration: 300, zIndex: 500, adaptiveSize: false };
            var o = $.extend(true, {}, config, param);
            var target = $(this);
            if (target.length == 0) return;
            $('#' + o.id).remove();
            var masker = null;

            //遮罩屏幕
            if (target.is(document) || target.is(window))
            {
                o.adaptiveSize = false;
                var position = 'fixed', width = '100%', height = '100%';
                //修正ie6
                if (IE6)
                {
                    var doc = document.documentElement;
                    position = 'absolute';
                    var documentSize = getIe6DocumentSize();
                    width = documentSize.width + 'px',
                    height = documentSize.height + 'px';
                }
                masker = '<div class="masker" id="' + o.id + '" style="display:none; position:' + position + '; z-index:' + o.zIndex + '; top:0; left:0; height:' + height + '; width:' + width + '; background-color:' + o.color + ';">';
                if (IE6) masker += '<iframe src="about:blank" style="width:100%;height:100%;position:absolute;top:0;left:0;z-index:-1;filter:alpha(opacity=0)" frameborder="no" border="no"></iframe>';
                masker += '</div>';
                masker = $(masker);
                masker.appendTo('body').fadeTo(o.duration, o.opacity);
                //ie6窗口大小变化时，调整遮罩层大小
                if (IE6)
                {
                    var resizeTimer = null;
                    $(window).on('resize.' + o.id, function ()
                    {
                        if (resizeTimer != null) clearTimeout(resizeTimer);
                        resizeTimer = setTimeout(function ()
                        {
                            masker.hide();
                            var documentSize = getIe6DocumentSize();
                            masker.css({ width: documentSize.width, height: documentSize.height }).show();
                        }, 150);
                    });
                }
            }
                //遮罩元素
            else
            {
                var position = 'absolute', height = target.outerHeight(), width = target.outerWidth(), top = target.offset().top, left = target.offset().left;
                masker = '<div class="masker" id="' + o.id + '" style="display:none; position:' + position + '; z-index:' + o.zIndex + '; top:' + top + 'px; left:' + left + 'px; height:' + height + 'px; width:' + width + 'px; background-color:' + o.color + ';">';
                //修正ie6
                if (IE6) masker += '<iframe src="about:blank" style="width:' + width + 'px;height:' + height + 'px;position:absolute;top:' + top + 'px; left:' + left + 'px;z-index:-1;filter:alpha(opacity=0)" frameborder="no" border="no"></iframe>';
                masker += '</div>';
                masker = $(masker);
                masker.appendTo('body').fadeTo(o.duration, o.opacity);
                //窗口大小变化时，调整遮罩层大小
                if (o.adaptiveSize)
                {
                    if (!IE6)
                    {
                        $(window).on('resize.' + o.id, function ()
                        {
                            masker.css({ width: target.outerWidth(), height: target.outerHeight() });
                        });
                    }
                    var resizeTimer = null;
                    $(window).on('resize.' + o.id, function ()
                    {
                        if (resizeTimer != null) clearTimeout(resizeTimer);
                        resizeTimer = setTimeout(function ()
                        {
                            masker.hide();
                            masker.css({ width: target.outerWidth(), height: target.outerHeight() }).show();
                        }, 150);
                    });
                }
                //ie6窗口滚动时，调整遮罩层位置
                if (IE6)
                {
                    var scrollTimer = null;
                    $(window).on('scroll.' + o.id, function ()
                    {
                        if (scrollTimer != null) clearTimeout(scrollTimer);
                        scrollTimer = setTimeout(function ()
                        {
                            var p = target.position();
                            masker.css({ top: p.top, left: p.left }).show();
                        }, 150);
                    });
                }
            }
            return masker;
        },
        /// @duration: 动画时间。默认0毫秒 
        unmask: function (duration)
        {
            duration = duration || 0;
            var masker = $(this);
            var id = masker.attr('id');
            masker.fadeTo(duration, 0, function ()
            {
                if (IE6) setTimeout(function () { masker.remove(); }, 100);
                else masker.remove();
                $(window).off('resize.' + id).off('scroll.' + id);
            });
        }
    })
});