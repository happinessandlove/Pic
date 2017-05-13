/**************************************************
 * v1.4
 * by 丁浩
 * 2015-03-19
 **************************************************/
$(function ()
{
    $.extend($.fn,
    {
        /// appendTo: 对话框添加至哪个元素内，默认为body
        /// follow: 相对定位的元素。符合jquery筛选器的值，或jquery对象，默认为document
        /// left：相对位置的左侧距离。如100、"100px"、"50%"、"outerLeft"、"left"、"innerLeft"、"center"、"innerRight"、"right"、"outerLeft"。默认值为center。当follow不为document时，可以为负数
        /// top：相对位置的上面距离。如100、"100px"、"50%"、"outerTop"、"top"、"innerTop"、"center"、"innerBottom"、"bottom"、"outerBottom"。默认值为center。当follow不为document时，可以为负数
        /// right：follow为document且left为false时有效，仅能为绝对值：100、"100px"
        /// bottom：follow为document且top为false时有效，仅能为绝对值：100、"100px"
        /// zIndex：层次。默认为1000
        /// maskerTarget：遮罩层的遮罩对象。符合jquery筛选器的值，默认值为null，此时遮罩对象为follow的值。为false时，不显示遮罩层。
        /// show：初始化完毕后是否显示。默认值为true
        /// esc：Esc键是否关闭弹出层。默认值为false
        /// doCloseEvent：是否执行默认的close事件。默认值为true
        /// closeButton：关闭按钮。符合jquery筛选器的值，默认值为“.layer-close”
        /// afterCloseEvent：关闭弹出层后执行的事件。默认值为null
        /// adaptivePosition: 指定是否在窗体调整大小时，调整浮动层的位置
        float: function (param, layerBox)
        {
            var config =
                {
                    appendTo: 'body'
                    , follow: document
                    , left: 'center'
                    , top: 'center'
                    , right: false
                    , bottom: false
                    , zIndex: 1000
                    , maskerTarget: null
                    , show: true
                    , esc: false
                    , doCloseEvent: true
                    , closeButton: '.layer-close'
                    , afterCloseEvent: null
                    , adaptivePosition: false
                };
            var o = $.extend(true, {}, config, param);
            o.follow = $(o.follow);
            var layer = layerBox || $(this);
            var container = $(o.appendTo);
            var isFixed, isIE6Fixed = false, baseOnRight = false, baseOnBottom = false;
            isFixed = o.follow.is(document) || o.follow.is(window);
            if (isFixed) o.adaptivePosition = false;
            if (isFixed && o.left === false)
            {
                if (o.right === false) return false;
                baseOnRight = true;
            }
            if (isFixed && o.top === false)
            {
                if (o.bottom === false) return false;
                baseOnBottom = true;
            }

            var eventId = 'float' + getRandom();

            var left = null, top = null;
            layer.css({ display: 'none', position: (isFixed && !IE6) ? 'fixed' : 'absolute', zIndex: o.zIndex }).addClass('float-layer');
            if (container.find(layer).length == 0)//动态弹出层
                layer.appendTo(container).data('layer-type', 'dynamic');
            else//静态弹出层
                layer.data('layer-type', 'static');
            setPosition();
            //显示弹出层、遮罩
            if (o.show)
            {
                if (o.maskerTarget !== false)
                {
                    if (o.maskerTarget === null) o.maskerTarget = o.follow;
                    var maskerId = 'masker' + getRandom();
                    layer.data('maskerId', maskerId);
                    $(o.maskerTarget).mask({ id: maskerId, adaptiveSize: o.adaptivePosition });
                }
                layer.show();
            }
            //相对于窗体时，窗体调整大小时重置弹出层位置。非ie6，或者指定resize为true时
            if ((isFixed || o.adaptivePosition) && !IE6)
            {
                $(window).on('resize.' + eventId, function ()
                {
                    setPosition();
                });
            }
                //窗体调整大小时重置弹出层位置。ie6，或者指定resize为true时
            else if ((isFixed || o.adaptivePosition) && IE6)
            {
                var resizeTimer = null;
                $(window).on('resize.' + eventId, function ()
                {
                    if (resizeTimer != null) clearTimeout(resizeTimer);
                    resizeTimer = setTimeout(function ()
                    {
                        setPosition();
                    }, 50);
                });
            }
            //相对于窗体时，滚动条滚动时修正弹出层位置。ie6
            if (isFixed && IE6)
            {
                var scrollTimer = null;
                $(window).on('scroll.' + eventId, function ()
                {
                    if (scrollTimer != null) clearTimeout(scrollTimer);
                    scrollTimer = setTimeout(function ()
                    {
                        setPosition();
                    }, 50);
                });
            }
            //Esc快捷键
            if (o.esc)
            {
                $(document).on('keydown.' + eventId, function (event)
                {
                    var target = event.target,
                        nodeName = target.nodeName,
                        rinput = /^INPUT|TEXTAREA$/,
                        keyCode = event.keyCode;
                    if (rinput.test(nodeName)) return;
                    if (keyCode === 27)
                        closeLayer();
                });
            }
            //关闭按钮事件
            if (o.doCloseEvent)
            {
                layer.find(o.closeButton).one('click', function () { closeLayer(); return false; });
            }
            function closeLayer()
            {
                var maskerId = layer.data('maskerId');
                layer.data('layer-type') == 'dynamic' ? layer.remove() : layer.hide();
                $(window).off('resize.' + eventId).off('scroll.' + eventId);
                $(document).off('keydown.' + eventId);
                if (o.maskerTarget != false)
                {
                    var masker = $('#' + maskerId);
                    masker.unmask();
                }
                if (o.afterCloseEvent != null) o.afterCloseEvent();
            }
            function setPosition()
            {
                getPosition();
                layer.css({ left: left, top: top });
            }
            function getPosition(layerWidth, layerHeight)
            {
                var layerW = layerWidth || layer.outerWidth(true), layerH = layerHeight || layer.outerHeight(true);
                var fw, fh, fl, ft;
                if (isFixed)
                {
                    if (typeof (o.left) === 'string' && o.left.toLowerCase().indexOf('left') >= 0) o.left = 'innerLeft';
                    else if (typeof (o.left) === 'string' && o.left.toLowerCase().indexOf('right') >= 0) o.left = 'innerRight';
                    if (typeof (o.top) === 'string' && o.top.toLowerCase().indexOf('top') >= 0) o.top = 'innerTop';
                    else if (typeof (o.top) === 'string' && o.top.toLowerCase().indexOf('bottom') >= 0) o.top = 'innerBottom';
                }
                //ie7及以上版本相对于document
                if (isFixed && !IE6)
                {
                    fw = $(window).width();
                    fh = $(window).height();
                    fl = 0;
                    ft = 0;
                }
                    //ie6相对于document，绝对定位
                else if (isFixed && IE6)
                {
                    fl = $(document).scrollLeft();
                    ft = $(document).scrollTop();
                    fw = $(window).width();
                    fh = $(window).height();
                }
                    //相对于某元素
                else
                {
                    fw = o.follow.outerWidth(true);
                    fh = o.follow.outerHeight(true);
                    if (o.appendTo !== 'body' && container != $('body'))
                    {
                        fl = 0;
                        ft = 0;
                    }
                    else
                    {
                        var fo = o.follow.offset();
                        fl = fo.left;
                        ft = fo.top;
                    }
                }
                left = convertValueToPx(o.left ? o.left : o.right, fl, fw, layerW, baseOnRight);
                top = convertValueToPx(o.top ? o.top : o.bottom, ft, fh, layerH, baseOnBottom);
            }
            function convertValueToPx(setValue, followOffset, followSize, layerSize, baseOnRightOrBottom)
            {
                var value = 0;
                if (typeof (setValue) === 'string') setValue = setValue.toLowerCase();
                if (typeof setValue === 'number')
                {
                    if (baseOnRightOrBottom)
                        value = followOffset + followSize - setValue - layerSize;
                    else
                        value = followOffset + setValue;
                }
                else if (setValue.lastIndexOf('px') === setValue.length - 2)
                {
                    if (baseOnRightOrBottom)
                        value = followOffset + followSize - parseInt(setValue) - layerSize;
                    else
                        value = followOffset + parseInt(setValue);
                }
                else if (setValue.lastIndexOf('%') === setValue.length - 1)
                    value = parseInt(followOffset + followSize * parseInt(setValue.split('%')[0]) / 100.0);
                else if (setValue === 'outerleft' || setValue === 'outertop')
                    value = followOffset - layerSize;
                else if (setValue === 'left' || setValue === 'top')
                    value = parseInt(followOffset - layerSize / 2.0);
                else if (setValue === 'innerleft' || setValue === 'innertop')
                    value = followOffset;
                else if (setValue === 'center')
                    value = parseInt(followOffset + (followSize - layerSize) / 2.0);
                else if (setValue === 'innerright' || setValue === 'innerbottom')
                    value = followOffset + followSize - layerSize;
                else if (setValue === 'right' || setValue === 'bottom')
                    value = parseInt(followOffset + followSize - layerSize / 2.0);
                else if (setValue === 'outerright' || setValue === 'outerbottom')
                    value = followOffset + followSize;
                return value;
            }
            //重置弹出层大小
            layer.SetSize = function (width, height, duration)
            {
                duration = duration || 500;
                getPosition(width, height);
                if (duration === 0)
                    layer.css({ left: left, top: top, height: height });
                else
                    layer.animate({ left: left, top: top, height: height, width: width }, duration);
                return layer;
            };
            //重置弹出层位置
            layer.SetPosition = function (l, t, duration)
            {
                duration = duration || 500;
                o.left = l; o.top = t;
                getPosition();
                layer.animate({ left: left, top: top }, duration);
                return layer;
            }
            //关闭弹出层
            layer.Close = function ()
            {
                closeLayer();
                return null;
            }
            return layer;
        }
    });
});
