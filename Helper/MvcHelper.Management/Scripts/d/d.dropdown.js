/**************************************************
 * v1.0
 * by 丁浩
 * 2014-10-27
 **************************************************/
$(function ()
{
    $.fn.extend(
    {
        dropdown:
            function (param)
            {
                var settings =
                    {
                        titleClass: 'd-dropdown-title'
                        , bodyClass: 'd-dropdown-body'
                        , triggerForm: 'hover'
                        , effect: 'show'
                        , hasIcon: true
                        , duration: { fadeIn: 300, fadeOut: 60, slideDown: 300, slideUp: 100 }
                        , hoverClass: 'd-dropdown-hover'
                    };

                return this.each(function ()
                {
                    var container = $(this);
                    var attrs =
                        {
                            titleClass: container.attr('titleClass')
                            , bodyClass: container.attr('bodyClass')
                            , triggerForm: container.attr('triggerform')
                            , effect: container.attr('effect')
                            , hasIcon: container.attr('hasIcon')
                            , hoverClass: container.attr('hoverClass')
                        };
                    var o = $.extend(true, {}, settings, attrs, param);
                    var title = container.children('.' + o.titleClass),
                        body = container.children('.' + o.bodyClass);
                    title.wrapInner('<div class="d-dropdown-title-border" />');
                    title = title.children('.d-dropdown-title-border');
                    body.wrapInner('<div class="d-dropdown-body-border" />');
                    body = body.children('.d-dropdown-body-border');
                    if (o.hasIcon)
                    {
                        title.wrapInner('<font class="d-dropdown-title-text" />');
                        title.append('<i class="d-dropdown-icon"></i>');
                        title.append('<span class="clr"></span>');
                    }
                    if (o.triggerForm == 'hover')
                    {
                        container.hover(function ()
                        {
                            if (o.effect == 'show')
                            {
                                $(this).addClass(o.hoverClass);
                            }
                            else if (o.effect == 'slide')
                            {
                                body.stop(false, true).slideDown(o.duration.slideDown);
                            }
                        }, function ()
                        {
                            if (o.effect == 'show')
                            {
                                $(this).removeClass(o.hoverClass);
                            }
                            else if (o.effect == 'slide')
                            {
                                body.stop(false, true).slideUp(o.duration.slideUp);
                            }
                        });
                    }
                    else if (o.triggerForm == 'click')
                    {
                        container.on('click', function ()
                        {
                            if (o.effect == 'show')
                            {
                                $(this).toggleClass(o.hoverClass);
                            }
                            else if (o.effect == 'slide')
                            {
                                if (body.is(':visible'))
                                    body.stop(false, true).slideUp(o.duration.slideUp);
                                else
                                    body.stop(false, true).slideDown(o.duration.slideDown);
                            }
                        });
                    }
                });
            }
    })
});

