var PushpinFactory = new function () {

    //Function for merging to simple objects
    function mergeObjects(first, second) {
        if (second != null) {
            for (key in second) {
                first[key] = second[key]
            }
        }
        return first;
    }

    //Calculates a pixel radius based on a values linear relation to a maximum value.
    //Radius is bounded by min/max radius values.
    function calculateRadius(value, maxValue, minRadius, maxRadius) {
        value = value || 0;

        var radius = value / maxValue * maxRadius;

        if (radius < minRadius) {
            radius = minRadius;
        } else if (radius > maxRadius) {
            radius = maxRadius;
        }

        return radius;
    }

    this.ColoredPin = function (location, options) {
        options.icon = options.icon || 'images/transparent_pin.png';
        options.width = options.width || 24;
        options.height = options.height || 37;
        options.fillColor = options.fillColor || new Microsoft.Maps.Color(255, 0, 175, 255);
        options.anchor = options.anchor || new Microsoft.Maps.Point(13, 37);

        var pinHTML = ['<svg height="',
                    options.width, '" width="', options.height,
                    '" version="1.0" xmlns="http://www.w3.org/2000/svg"><circle cx="13" cy="13" r="11" style="fill:',
                    options.fillColor.toHex(),
                    ';fill-opacity:1;"/></svg><img style="position:absolute;top:0;left:0;" src="', options.icon, '"/>'];

        if (options.text) {
            pinHTML.push('<span style="position:absolute;left:0px;color:#fff;font-size:12px;text-align:center;width:', options.width, 'px;top:5px;">', options.text, '</span>');
        }

        options.htmlContent = pinHTML.join('');

        return new Microsoft.Maps.Pushpin(location, options);
    };

    this.FontPins = function (fontStyle) {
        this.Create = function (location, size, options) {
            size = size || 12;
            options = options || {};
            options.fillColor = options.fillColor || defaultFillColor;

            var html = ['<div style="text-align:center;width:', size, 'px"><span style="font-family:\'', fontStyle, '\';font-size:', size,
                'px;width:', size * 2, 'px;text-align:center;color:', options.fillColor.toHex(), ';">', options.icon, '</span>'];

            if (options.text) {
                html.push('<span style="position:absolute;left:0px;color:#fff;font-size:12px;text-align:center;width:', size, 'px;top:', size / 2 - 6, 'px;">', options.text, '</span>');
            }

            html.push('</div>');

            return new Microsoft.Maps.Pushpin(location, {
                height: size,
                width: size,
                htmlContent: html.join('')
            });
        };
    };

    this.ScaledCircles = function (minRadius, maxRadius, maxValue) {

        maxValue = maxValue || 100;
        minRadius = minRadius || 1;
        maxRadius = maxRadius || 50;

        var defaultFillColor = new Microsoft.Maps.Color(150, 0, 175, 255),
            defaultStrokeColor = new Microsoft.Maps.Color(150, 130, 130, 130),
            defaultStrokeThickness = 2;

        function createCircleHTML(radius, options) {
            options = options || {};
            options.fillColor = options.fillColor || defaultFillColor;
            options.strokeColor = options.strokeColor || defaultStrokeColor;
            options.strokeThickness = options.strokeThickness || defaultStrokeThickness;

            var circleHTML = ['<div"><svg height="',
                    radius * 2, '" width="', radius * 2,
                    '" version="1.0" xmlns="http://www.w3.org/2000/svg"><circle cx="',
                    radius, '" cy="', radius, '" r="', radius,
                    '" style="fill:', options.fillColor.toHex(),
                    ';stroke:', options.strokeColor.toHex(),
                    ';stroke-width:', options.strokeThickness,
                    ';fill-opacity:', options.fillColor.getOpacity(),
                    ';stroke-opacity:', options.strokeColor.getOpacity(),
                    ';"/></svg>'];

            if (options.text) {
                circleHTML.push('<span style="position:absolute;left:0px;color:#fff;font-size:12px;text-align:center;width:', radius * 2, 'px;top:', radius - 6, 'px;">', options.text, '</span>');
            }

            circleHTML.push('</div>');

            return circleHTML.join('');
        }

        this.Create = function (location, value, options) {
            var radius = calculateRadius(value, maxValue, minRadius, maxRadius);

            var pin = new Microsoft.Maps.Pushpin(location, {
                anchor: new Microsoft.Maps.Point(radius, radius),
                height: radius * 2,
                width: radius * 2,
                htmlContent: createCircleHTML(radius, options)
            });

            pin.setValue = function (val, opt) {
                value = val;
                radius = calculateRadius(value, maxValue, minRadius, maxRadius);
                options = mergeObjects(options, opt);

                pin.setOptions({ htmlContent: createCircleHTML(radius, options) });
            };

            pin.getValue = function(){
                return value;
            };

            return pin;
        };
    };
    
    this.PieCharts = function (colorMap, minRadius, maxRadius, maxValue) {

        maxValue = maxValue || 100;
        minRadius = minRadius || 1;
        maxRadius = maxRadius || 50;

        var defaultStrokeColor = new Microsoft.Maps.Color(150, 130, 130, 130),
            defaultStrokeThickness = 2;       

        function createArc(cx, cy, r, startAngle, angle, fillColor, strokeColor, strokeThickness, text) {
            var x1 = cx + r * Math.sin(startAngle);
            var y1 = cy - r * Math.cos(startAngle);
            var x2 = cx + r * Math.sin(startAngle + angle);
            var y2 = cy - r * Math.cos(startAngle + angle);
          
            //Flag for when arc's are larger than 180 degrees
            var big = 0;
            if (angle > Math.PI) {
                big = 1;
            }

            var path = ['<path d="M ', cx, ' ', cy, ' L ', x1, ' ', y1, ' A ', r, ',', r, ' 0 ', big, ' 1 ', x2, ' ', y2,
                ' Z" style="fill:', fillColor.toHex(),
                ';stroke:', strokeColor.toHex(),
                ';stroke-width:', strokeThickness,
                ';fill-opacity:', fillColor.getOpacity(),
                ';stroke-opacity:', strokeColor.getOpacity(),
                ';"'];
              
            if(text){
                path.push('onmouseover="PushpinFactory.ShowTooltip(evt, \'', text, '\');" onmouseout="PushpinFactory.HideTooltip(evt)"');
            }

            path.push('/>');

            return path.join('');
        }

        function createPieChartHTML(values, total, numSlices, radius, options) {
            options = options || {};
            options.strokeColor = options.strokeColor || defaultStrokeColor;
            options.strokeThickness = options.strokeThickness || defaultStrokeThickness;

            var pieChart = ['<div style="overflow:visible;"><svg height="',
                    radius * 2, '" width="', radius * 8,
                    '" version="1.0" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink">'];

            var cx = radius, cy = radius, startAngle = 0, angle = 0, text;

            for (var i = 0; i < numSlices; i++) {
                angle = (Math.PI * 2 * (values[i] / total));
               
                if (angle > 2 * Math.PI - 0.001)
                    angle = angle - 0.01;
                text = (options.text && i <= options.text.length) ? options.text[i] : null;

                pieChart.push(createArc(cx, cy, radius, startAngle, angle, colorMap[i], options.strokeColor, options.strokeThickness, text));
                startAngle += angle;
            }
            
            if (options.text) {
                pieChart.push('<rect style="fill:white;stroke:black;stroke-width:1;opacity:0.8;" x="');
                pieChart.push(radius -4, '" y="', radius - 13, '" rx="4" ry="4" width="55" height="17" visibility="hidden"/>');
                pieChart.push('<text style="font-size: 12px;" x="', radius, '" y="', radius, '" visibility="hidden">Tooltip</text>');
            }

            pieChart.push('</svg></div>');

            return pieChart.join('');
        }

        this.Create = function (location, values, options) {
            //Calculate number of slices of pie.
            var numSlices = Math.min(values.length, colorMap.length);

            //Calculate the total of the data in the pie chart
            var total = 0;
            for (var i = 0; i < values.length; i++) {
                total += values[i];
            }

            var radius = calculateRadius(total, maxValue, minRadius, maxRadius);

            var pin = new Microsoft.Maps.Pushpin(location, {
                anchor: new Microsoft.Maps.Point(radius, radius),
                height: radius * 2,
                width: radius * 2,
                htmlContent: createPieChartHTML(values, total, numSlices, radius, options),
                zindex: 2
            });

            pin.setValues = function (val, opt) {
                values = val;

                numSlices = Math.min(values.length, colorMap.length);

                total = 0;
                for (var i = 0; i < values.length; i++) {
                    total += values[i];
                }

                radius = calculateRadius(total, maxValue, minRadius, maxRadius);
                options = mergeObjects(options, opt);

                pin.setOptions({ htmlContent: createPieChartHTML(values, total, numSlices, radius, options) });
            };

            pin.getValues = function () {
                return values;
            };

            return pin;
        };
    };
    
    this.ShowTooltip = function (evt, mouseovertext) {
        var textNodes = evt.target.parentNode.getElementsByTagName('text');

        if (textNodes && textNodes.length > 0) {
            var tooltip = textNodes[0];
            tooltip.firstChild.data = mouseovertext;
            tooltip.setAttributeNS(null, "visibility", "visible");

            var rectNodes = evt.target.parentNode.getElementsByTagName('rect');

            if (rectNodes && rectNodes.length > 0) {
                var tooltip_bg = rectNodes[0];

                var length = tooltip.getComputedTextLength();
                tooltip_bg.setAttributeNS(null, "width", length + 8);
                tooltip_bg.setAttributeNS(null, "visibility", "visibile");
            }
        }
    };

    this.HideTooltip = function (evt) {
        var textNodes = evt.target.parentNode.getElementsByTagName('text');

        if (textNodes && textNodes.length > 0) {
            var tooltip = textNodes[0];
            tooltip.setAttributeNS(null, "visibility", "hidden");
        }

        var rectNodes = evt.target.parentNode.getElementsByTagName('rect');

        if (rectNodes && rectNodes.length > 0) {
            var tooltip_bg = rectNodes[0];
            tooltip_bg.setAttributeNS(null, "visibility", "hidden");
        }
    };
};
