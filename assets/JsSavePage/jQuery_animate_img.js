(function ($) {
    $.fn.ImgAnimation = function (options) {
        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            Row1: "1px",
            Row2: "105px",
            col1: "1px",
            col2: "105px",
            col3: "209px",
            dcol1: "1px",
            dcol2: "105px",
            dcol3: "209px",
            smimgheight: "100",
            SpanClick: "Demo",
            SpanBig: "DemoBig",
            Span1: "Span1",
            Span2: "Span2",
            Span3: "Span3",
            Span4: "Span4",
            Span5: "Span5"
        }, options);
        //ImgAnimation the collection based on the settings variable.
        $("." + settings.SpanClick).click(function () {
            debugger;
            var i = this.id;

            var pos = 0;
            if ($(this).height() <= settings.smimgheight) {

                if (!$(this).hasClass(settings.SpanBig)) {
                    $("." + settings.SpanClick).stop(true, true).removeClass(settings.SpanBig, 500, "easeOutBounce");
                    var jl;
                    var jt;
                    if ($(this).css("marginLeft") == settings.col1) {
                        settings.col2 = settings.dcol2;
                        settings.col3 = settings.dcol3;
                        jl = settings.col1;
                        jt = $(this).css('marginTop');
                    }
                    else if ($(this).css("marginLeft") == settings.col2) {
                        settings.col2 = settings.dcol2.replace("px", "")
                        settings.col2 = (parseInt(settings.col2) - 178) + "px";
                        settings.col3 = settings.dcol3;
                        jl = settings.col2;
                        jt = $(this).css('marginTop');
                    }
                    else if ($(this).css("marginLeft") == settings.col3) {
                        settings.col2 = settings.dcol2.replace("px", "")
                        settings.col3 = settings.dcol3.replace("px", "")
                        settings.col2 = (parseInt(settings.col2) - 178) + "px";
                        settings.col3 = (parseInt(settings.col3) - 178) + "px";
                        jl = settings.col3;
                        jt = $(this).css('marginTop');
                    }
                    else {
                        jl = $(this).css('marginLeft');
                        jt = $(this).css('marginTop');
                    }




                    if (i == settings.Span1) {
                        if (jl == settings.col1 && jt == settings.Row1) {
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row1) {
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col1 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row1) {
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                    }


                    if (i == settings.Span2) {
                        if (jl == settings.col1 && jt == settings.Row1) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col1 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                        }
                        else if (jl == settings.col2 && jt == settings.Row1) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row1) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row2) {
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                    }


                    if (i == settings.Span3) {
                        if (jl == settings.col1 && jt == settings.Row1) {

                        }
                        else if (jl == settings.col1 && jt == settings.Row2) {

                        }
                        else if (jl == settings.col2 && jt == settings.Row1) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });

                        }
                        else if (jl == settings.col2 && jt == settings.Row2) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row1) {
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row2) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                    }

                    if (i == settings.Span4) {
                        if (jl == settings.col1 && jt == settings.Row1) {
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col1 && jt == settings.Row2) {
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row1) {
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row2) {
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row1) {

                        }
                        else if (jl == settings.col3 && jt == settings.Row2) {

                        }

                    }

                    if (i == settings.Span5) {

                        if (jl == settings.col1 && jt == settings.Row1) {

                        }
                        else if (jl == settings.col1 && jt == settings.Row2) {

                        }
                        else if (jl == settings.col2 && jt == settings.Row1) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col2 && jt == settings.Row2) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });

                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row1) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                        else if (jl == settings.col3 && jt == settings.Row2) {
                            $("#" + settings.Span5).stop(true, true).animate({
                                marginLeft: settings.col3,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span1).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row2
                            });

                            $("#" + settings.Span3).stop(true, true).animate({
                                marginLeft: settings.col2,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span4).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row1
                            });
                            $("#" + settings.Span2).stop(true, true).animate({
                                marginLeft: settings.col1,
                                marginTop: settings.Row2
                            });
                        }
                    }

                    $(this).stop(true, true).addClass(settings.SpanBig, 1000, "easeOutBounce");
                }
            }
            return false;
        });
    };
}(jQuery));