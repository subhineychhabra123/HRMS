(function () {

    $.fn.popbox = function (options) {
        var self = $(this);
        var pop = this;
        var box = self.find('._popboxcontainer');
        var settings = $.extend({
            selector: this.selector,
            open: '.open',
            arrow: '.arrow',
            arrow_border: '.arrow-border',
            close: '.close',
            width: '',
            height: 'auto',
            overlay: '.popup_overlay',
            zindex: 99999
        }, options);
        var overLayId = this.selector + '_OverLay';
        overLayId = overLayId.replace(/[.#]/, "");
        pop.methods = { 
            open: function (event) {
              
                // event.preventDefault();                
                var pop = $(this);
                box.find(settings['arrow']).css({ 'left': box.width() / 2 - 10 });
                box.find(settings['arrow_border']).css({ 'left': box.width() / 2 - 10 });
                self.css({ 'z-index': settings['zindex'] });
                if (box.css('display') == 'block') {
                   // pop.methods.close();
                } else {
                    var overLayer = $('<div id=' + overLayId + ' class="over-lay">')
                    $("#" + overLayId).remove();

                    overLayer.css({ 'z-index': settings['zindex'] - 10 }).prependTo("body");
                    $(settings['overlay']).css({ 'display': 'block', 'z-index': settings['zindex'] - 2 })
                    //box.css({ 'display': 'block', 'top': 10, 'left': ((pop.parent().width() / 2) - box.width() / 2) });
                    box.css({ 'display': 'inline-block' }).css({ 'width': settings['width'], height: settings['height'] });

                }
            },

            close: function () {
              
                box.fadeOut("fast");
                $("#" + overLayId).remove();
                $(settings['overlay']).css({ 'display': 'none' })
            }
        };

        $(document).bind('keyup', function (event) {
            if (event.keyCode == 27) {
                pop.methods.close();
            }
        });

        $(document).bind('click', function (event) {
            if (!$(event.target).closest(settings['selector']).length) {
                //methods.close();
            }
        });

        return this.each(function () {
            $(this).css({ 'width': $(settings['box']).width() }); // Width needs to be set otherwise popbox will not move when window resized.
            $(document).on('click', settings['open'], pop.methods.open);
            $(document).find(settings['close']).bind('click', pop.methods.close);
            pop;
        });
    }

}).call(this);