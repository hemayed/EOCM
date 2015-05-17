(function ($) {
    var map,
       mapDiv,
       jqMapDiv,
       that,
       actionType,
       step,
       points,
       lineDrawingStyle,
       polyDrawingStyle,
       polyFillStyle,
       pushpins,
       polylines,
       shapes,
       mapClickEventHandler = null,
       mapMouseDownEventHandler = null,
       mapMouseUpEventHandler = null,
       mapMouseMoveEventHandler = null,
       mapClickEventName = 'click',
       mapMousedownEventName = 'mousedown',
       mapMouseupEventName = 'mouseup',
       mapMousemoveEventName = 'mousemove',
       logger;

    //console.log("Start up");
    var MM = Microsoft.Maps;

    var bingUtils = {
        BingMapsHelper: function () {
            //console.log("BingMapsHelper");
            this.EarthRadiusInMiles = 3956.0;
            this.EarthRadiusInKilometers = 6367.0;
            this.rad = Math.PI / 180;
            this.invRad = 180 / Math.PI;
            this.angleToRadian = function (angle) {
                return angle * this.rad;
            };
            this.radianToAngle = function (radian) {
                return radian * this.invRad;
            };
            this.edgeSegment = 0.1;
            this.useEdgeSegment = false;

            this.getLocationsOnCircle = function (pointA, pointB) {
                var location1 = map.tryPixelToLocation(pointA);
                var location2 = map.tryPixelToLocation(pointB);

                var lat = this.angleToRadian(location1.latitude);
                var lon = this.angleToRadian(location1.longitude);
                var locs = [];
                var distanceInRadians = this.getDistanceInRadians(location1, location2);
                var latRadians, lngRadians;
                if (this.useEdgeSegment && this.edgeSegment) {
                    logger.log("Creating Circle with specified edge segment length of: "
                        + this.edgeSegment);
                    var limit = Math.ceil(2 * Math.PI / this.edgeSegment)
                        + this.edgeSegment;
                    logger.log("Limit is: " + limit);
                    var rds, rd; //, i = 0;
                    for (rds = 0; rds <= limit; rds += this.edgeSegment) {
                        rd = this.edgeSegment * rds;
                        //i++;
                        latRadians = Math.asin(Math.sin(lat) * Math.cos(distanceInRadians)
                            + Math.cos(lat) * Math.sin(distanceInRadians) * Math.cos(rd));
                        lngRadians = lon + Math.atan2(Math.sin(rd) * Math.sin(distanceInRadians) * Math.cos(lat),
                            Math.cos(distanceInRadians) - Math.sin(lat) * Math.sin(latRadians));
                        locs.push(new MM.Location(this.radianToAngle(latRadians),
                            this.radianToAngle(lngRadians)));
                    }
                } else {
                    logger.log("Creating Circle with 1 deg step");
                    for (var deg = 0; deg <= 360; deg += 1) {
                        var degAsRadian = this.angleToRadian(deg);
                        latRadians = Math.asin(Math.sin(lat) * Math.cos(distanceInRadians)
                            + Math.cos(lat) * Math.sin(distanceInRadians) * Math.cos(degAsRadian));
                        lngRadians = lon + Math.atan2(Math.sin(degAsRadian) * Math.sin(distanceInRadians) * Math.cos(lat),
                            Math.cos(distanceInRadians) - Math.sin(lat) * Math.sin(latRadians));
                        locs.push(new MM.Location(this.radianToAngle(latRadians), this.radianToAngle(lngRadians)));
                    }
                }
                logger.log("Created " + locs.length + " points.");
                return locs;
            };

            this.getLocationsOnCircleWithRadius = function (originPoint, radius, unit) {
                logger.log("Get Locations on Circle with given Radius");
                logger.log("Origin: " + originPoint + ", Radius: " + radius + ", Unit: " + unit);
                var earthRadius = unit === "m" ? this.EarthRadiusInMiles : this.EarthRadiusInKilometers;
                var originLocation = map.tryPixelToLocation(originPoint);
                var distanceAsAngle = parseFloat(radius) / earthRadius;
                var lat = this.angleToRadian(originLocation.latitude);
                var lon = this.angleToRadian(originLocation.longitude);
                var latRadians = Math.asin(Math.sin(lat) * Math.cos(distanceAsAngle)
                    + Math.cos(lat) * Math.sin(distanceAsAngle) * Math.cos(0));
                var lngRadians = lon + Math.atan2(Math.sin(0) * Math.sin(distanceAsAngle) * Math.cos(lat),
                    Math.cos(distanceAsAngle) - Math.sin(lat) * Math.sin(latRadians));
                var secondLocation = new MM.Location(this.radianToAngle(latRadians),
                    this.radianToAngle(lngRadians));
                var secondPoint = map.tryLocationToPixel(secondLocation);
                return this.getLocationsOnCircle(originPoint, secondPoint);
            };

            this.getDistanceInRadians = function (location1, location2) {
                return 2 * Math.asin(Math.min(1, Math.sqrt((Math.pow(Math.sin((this.angleToRadian(location2.latitude)
                    - this.angleToRadian(location1.latitude)) / 2.0), 2.0)
                    + Math.cos(this.angleToRadian(location1.latitude)) * Math.cos(this.angleToRadian(location2.latitude)) * Math.pow(Math.sin((this.angleToRadian(location2.longitude)
                        - this.angleToRadian(location1.longitude)) / 2.0), 2.0)))));
            };
        }
    };

    $.widget('cm-widgets.bingMaps7', {

        options: {
            width: 1000,
            height: 800,
            centerX: 38.809771,
            centerY: -77.0321543125,
            //38.889824, -77.008938
            zoom: 12,
            appKey: null,
            dataUrl: '',
            step: 20,
            lineDrawingStyle: { strokeColor: new MM.Color(255, 0, 0, 255), strokeThickness: 8 },
            polyDrawingStyle: { strokeColor: new MM.Color(255, 0, 0, 255), strokeThickness: 1 },
            polyFillStyle: {
                strokeColor: new MM.Color(255, 0, 0, 255),
                strokeThickness: 1, fillColor: new MM.Color(80, 0, 255, 0)
            },
            polyLocationsDrawingStyle: { strokeColor: new MM.Color(255, 128, 128, 128), strokeThickness: 2 },
            polyLocationsFillStyle: {
                strokeColor: new MM.Color(255, 0, 0, 255),
                strokeThickness: 1, fillColor: new MM.Color(80, 0, 0, 255)
            },
            logger: new cmUtils.Logger()
        },

        _create: function () {
            logger = this.options.logger;
            logger.enabled = false;
            that = this;
            var name = this.name, options = this.options, elem = this.element.context;
            step = options.step;
            lineDrawingStyle = options.lineDrawingStyle;
            polyDrawingStyle = options.polyDrawingStyle;
            polyFillStyle = options.polyFillStyle;
            polyLocationsDrawingStyle = options.polyLocationsDrawingStyle;
            polyLocationsFillStyle = options.polyLocationsFillStyle;
            var mapOptions = {
                credentials: options.appKey, zoom: options.zoom,
                center: new MM.Location(options.centerX, options.centerY)
            };
            mapDiv = document.getElementById(elem.id);
            mapDiv.style.height = options.height + "px";
            mapDiv.style.width = options.width + "px";
            map = new MM.Map(mapDiv, mapOptions);
            jqMapDiv = $('#' + mapDiv.id);

            pushpins = new MM.EntityCollection();
            polylines = new MM.EntityCollection();
            shapes = new MM.EntityCollection();

            map.entities.push(shapes);
            map.entities.push(polylines);
            map.entities.push(pushpins);

            logger.log("created " + name);
        },

        drawLine: function () {
            that._initializeMapAction("drawLine");
            points = [];

            var handleLineDrawingMousedown = function (e) {
                if (e.targetType !== "map") return;
                logger.log(e.eventName);
                var point = new MM.Point(e.getX(), e.getY());
                points.push(point);
                that._createPushpin(point);
                that._addHandler(mapMousemoveEventName, handleLineDrawingMousemove);
            }

            var handleLineDrawingMouseup = function (e) {
                logger.log(e);
                logger.log(e.eventName);
                //fix for chrome, ff and safari
                //if (e.targetType !== "map") return;
                if (e.eventName !== 'mouseup') {
                    logger.log("unbinding fixer");
                    jqMapDiv.unbind('mouseup', fixerHandler); return;
                }
                var point = new MM.Point(e.getX(), e.getY());
                if (!that._arePointsEqual(point, points[0])) {
                    logger.log("distance between points ok, adding second pushpin...");
                    if (points.lenght > 1) { points.pop(); }
                    points.push(point);
                    that._createPushpin(point);
                }
                else {
                    logger.log("distance between points too short, removing first pushpin...");
                    points.pop();
                    that._popPushpin();
                }
                that._detachEventHandlers();
                map.setOptions({ disablePanning: false, disableZooming: false });
            }

            var handleLineDrawingMousemove = function (e) {
                if (e.targetType !== "map") return;
                logger.log(e.eventName);
                var point = new MM.Point(e.getX(), e.getY());
                var lastPoint = (points.length === 1) ? points[0] : points[points.length - 2];
                logger.log(points.length);
                logger.log(point + "," + lastPoint);
                if (!that._arePointsEqual(point, lastPoint)) {
                    var withReplace = points.length > 1;
                    if (withReplace) points.pop();
                    points.push(point);
                    that._createLine(points[0], point, withReplace);
                }
            }

            that._addHandler(mapMousedownEventName, handleLineDrawingMousedown);
            that._addHandler(mapMouseupEventName, handleLineDrawingMouseup);
            //fix for chrome, ff and safari
            var fixerHandler = function (ev) { handleLineDrawingMouseup(ev); };
            jqMapDiv.bind('mouseup', fixerHandler);

        },

        drawPolygon: function () {
            that._initializeMapAction("drawPolygon");

            points = [];
            var firstPoint = null;
            var lastPoint = null;
            var linePoints = [];
            var isCapturingMouse = false;

            var handleLineDrawingMousedown = function (e) {
                if (e.targetType !== "map") return;
                logger.log(e.eventName);
                var point = new MM.Point(e.getX(), e.getY());
                if (firstPoint === null) {
                    points.push(point);
                    linePoints = [];
                    linePoints.push(point);
                    firstPoint = point;
                    that._createPushpin(point);
                    that._addHandler(mapMousemoveEventName, handleLineDrawingMousemove);
                    isCapturingMouse = true;
                }
                else if (that._arePointsEqual(point, lastPoint)) {
                    logger.log("clicked on the last point");
                    linePoints = [];
                    linePoints.push(lastPoint);
                    that._addHandler(mapMousemoveEventName, handleLineDrawingMousemove);
                    isCapturingMouse = true;
                }

            }

            var handleLineDrawingMouseup = function (e) {
                if (!isCapturingMouse) return;
                //fix for chrome, ff and safari
                //if (e.targetType !== "map") return;
                logger.log(e);
                logger.log(e.eventName);
                if (mapMouseMoveEventHandler !== null) MM.Events.removeHandler(mapMouseMoveEventHandler);
                if (e.eventName !== 'mouseup') {
                    logger.log("unbinding fixer");
                    jqMapDiv.unbind('mouseup', fixerHandler);
                    return;
                }
                var point = new MM.Point(e.getX(), e.getY());
                if (!that._arePointsEqual(point, linePoints[0])) {
                    if (linePoints.length > 1) { linePoints.pop(); }
                    if (points.length > 2 && that._arePointsEqual(firstPoint, point)) {
                        point = new MM.Point(firstPoint.x, firstPoint.y);
                        points.push(point);
                        that._detachEventHandlers();
                        map.setOptions({ disablePanning: false, disableZooming: false });
                        logger.log("Closing Poly....");
                        that._createLine(firstPoint, point, true, polyDrawingStyle);
                        that._createShape(points);
                        return; //done

                    }
                    lastPoint = point;
                    points.push(lastPoint);
                    that._createPushpin(lastPoint);
                    isCapturingMouse = false;
                }
                else {
                    logger.log("distance between points too short, removing first pushpin...");
                    linePoints.pop();
                    isCapturingMouse = false;
                    if (lastPoint === null) {
                        points.pop();
                        that._popPushpin();
                        that._detachEventHandlers();
                        map.setOptions({ disablePanning: false, disableZooming: false });
                        return;
                    }
                }
            }

            var handleLineDrawingMousemove = function (e) {
                if (!isCapturingMouse) return;
                if (e.targetType !== "map") return;
                logger.log(e.eventName);
                var point = new MM.Point(e.getX(), e.getY());
                var lastLinePoint = (linePoints.length === 1) ? linePoints[0] : linePoints[linePoints.length - 2];
                logger.log(linePoints.length);
                logger.log(point + "," + lastLinePoint);
                if (!that._arePointsEqual(point, lastLinePoint)) {
                    var withReplace = linePoints.length > 1;
                    if (withReplace) linePoints.pop();
                    linePoints.push(point);
                    that._createLine(lastLinePoint, point, withReplace, polyDrawingStyle);
                }
            }

            that._addHandler(mapMousedownEventName, handleLineDrawingMousedown);
            that._addHandler(mapMouseupEventName, handleLineDrawingMouseup);
            //fix for chrome, ff and safari
            var fixerHandler = function (ev) { handleLineDrawingMouseup(ev); };
            jqMapDiv.bind('mouseup', fixerHandler);
        },

        drawRectangle: function () {
            that._initializeMapAction("drawRectangle");

            points = [];
            var point1, point2, lastPoint;
            var isCapturingMouse = false;

            var handleRectangleDrawing = function (e) {
                logger.log("handleRectangleDrawing");
                if (e.targetType !== "map") return;
                logger.log(e.eventName);

                if (e.eventName === "mousedown") {
                    point1 = new MM.Point(e.getX(), e.getY());
                    lastPoint = point1;
                    isCapturingMouse = true;
                    that._addHandler(mapMousemoveEventName, handleRectangleDrawing);
                }
                else if (isCapturingMouse && e.eventName === "mousemove") {
                    point2 = new MM.Point(e.getX(), e.getY());
                    if (!that._arePointsEqual(lastPoint, point2)) {
                        points = getRectangleVertices(point1, point2);
                        points.push(points[0].clone());
                        logger.log("Poly Line Points Count: " + points.length);
                        that._createPolyLine(points, lastPoint !== point1);
                        lastPoint = point2;
                    }
                }
                else if (e.eventName === "mouseup") {
                    that._detachEventHandlers();
                    logger.log("Finshing Rectangle ....");
                    isCapturingMouse = false;
                    point2 = new MM.Point(e.getX(), e.getY());
                    var withReplace = false;
                    if (!that._arePointsEqual(lastPoint, point2)) {
                        lastPoint = point2;
                        points = getRectangleVertices(point1, point2);
                        withReplace = true;
                    }
                    var drawRectangle = points.length > 0;
                    if (drawRectangle) {
                        for (var i = 0; i < points.length; i++) {
                            that._createPushpin(points[i]);
                        }
                        points.push(points[0].clone());
                        that._createPolyLine(points, withReplace);
                        that._createShape(points);
                    }
                    else {
                        logger.log("No rectangle to draw....;");
                    }
                    map.setOptions({ disablePanning: false, disableZooming: false });
                }
            }

            function getRectangleVertices(pointA, pointC) {
                var pointB = new MM.Point(pointC.x, pointA.y);
                var pointD = new MM.Point(pointA.x, pointC.y);
                logger.log("Rectangle:");
                logger.log("A: " + pointA);
                logger.log("B: " + pointB);
                logger.log("C: " + pointC);
                logger.log("D: " + pointD);
                return [pointA, pointB, pointC, pointD];
            }

            that._addHandler(mapMousedownEventName, handleRectangleDrawing);
            that._addHandler(mapMouseupEventName, handleRectangleDrawing);
        },

        drawCircle: function () {
            that._initializeMapAction("drawCircle");

            points = [];
            var point1, point2, lastPoint;;
            var isCapturingMouse = false;

            var handleCircleDrawing = function (e) {
                logger.log("handleCircleDrawing");
                if (e.targetType !== "map") return;
                logger.log(e.eventName);

                if (e.eventName === "mousedown") {
                    point1 = new MM.Point(e.getX(), e.getY());
                    lastPoint = point1;
                    isCapturingMouse = true;
                    that._createPushpin(point1);
                    that._addHandler(mapMousemoveEventName, handleCircleDrawing);
                }
                else if (isCapturingMouse && e.eventName === "mousemove") {
                    point2 = new MM.Point(e.getX(), e.getY());
                    if (!that._arePointsEqual(lastPoint, point2)) {
                        logger.log("need to add new point");
                        that._createLine(point1, point2, (lastPoint !== point1), polyLocationsDrawingStyle);
                        lastPoint = point2;
                    }
                }
                else if (e.eventName === "mouseup") {
                    that._detachEventHandlers();
                    logger.log("Finshing Circle ....");
                    isCapturingMouse = false;
                    point2 = new MM.Point(e.getX(), e.getY());
                    var withReplace = false;

                    if (!that._arePointsEqual(lastPoint, point2)) {
                        lastPoint = point2;
                        withReplace = true;
                    }
                    var drawCircle = !that._arePointsEqual(point1, lastPoint);
                    if (drawCircle) {
                        that._createPushpin(lastPoint);
                        that._createLine(point1, lastPoint, withReplace, polyLocationsDrawingStyle);
                        var bingHelper = new bingUtils.BingMapsHelper();
                        //bingHelper.edgeSegment = 0.05;
                        //bingHelper.useEdgeSegment = true;
                        var locations = bingHelper.getLocationsOnCircle(point1, lastPoint);
                        //that._createPolyLineFromLocations(locations, false);
                        that._createShapeFromLocations(locations);
                    }
                    else {
                        logger.log("No circle to draw....;");
                        that._popPushpin();
                    }
                    map.setOptions({ disablePanning: false, disableZooming: false });
                }
            }

            that._addHandler(mapMousedownEventName, handleCircleDrawing);
            that._addHandler(mapMouseupEventName, handleCircleDrawing);

        },

        drawCircleWithRadius: function (radius, unit) {
            that._initializeMapAction("drawCircleWithRadius");

            points = [];
            var centerPoint;

            var handleCircleDrawing = function (e) {
                logger.log("handleCircleDrawing");
                if (e.targetType !== "map") return;
                logger.log(e.eventName);

                if (e.eventName === "click") {
                    if (!radius || radius <= 0) {
                        logger.log("No circle to draw....;");
                        return;
                    }
                    that._detachEventHandlers();
                    centerPoint = new MM.Point(e.getX(), e.getY());
                    that._createPushpin(centerPoint);
                    var bingHelper = new bingUtils.BingMapsHelper();
                    //bingHelper.edgeSegment = 0.05;
                    //bingHelper.useEdgeSegment = true;
                    var locations = bingHelper.getLocationsOnCircleWithRadius(centerPoint, radius, unit);
                    //that._createPolyLineFromLocations(locations, false);
                    that._createShapeFromLocations(locations);
                    map.setOptions({ disablePanning: false, disableZooming: false });
                }
            }

            that._addHandler(mapClickEventName, handleCircleDrawing);

        },

        _initializeMapAction: function (actionName) {
            logger.log(actionName);
            map.setOptions({ disablePanning: true, disableZooming: true });
            that._detachEventHandlers();
        },

        _addHandler: function (evtName, handler) {
            logger.log("_addHandler " + evtName);
            switch (evtName) {
                case 'mousedown':
                    mapMouseDownEventHandler = MM.Events.addHandler(map, evtName, handler);
                    break;
                case 'mouseup':
                    mapMouseUpEventHandler = MM.Events.addHandler(map, evtName, handler);
                    break;
                case 'mousemove':
                    mapMouseMoveEventHandler = MM.Events.addHandler(map, evtName, handler);
                    break;
                case 'click':
                    mapClickEventHandler = MM.Events.addHandler(map, evtName, handler);
                    break;
            }
        },

        _arePointsEqual: function (point1, point2) {
            var deltaX = point1.x - point2.x;
            var deltaY = point1.y - point2.y;
            var distance = 0.5 * (Math.sqrt(Math.pow(deltaX, 2) + Math.pow(deltaY, 2)));
            logger.log("distance - dx=" + deltaX + ", dy=" + deltaY + ": distance=" + distance + ", step=" + step);
            return distance < step;
        },

        _createPushpin: function (point) {
            logger.log("_createPushpin");
            var loc = map.tryPixelToLocation(point);
            pushpins.push(new MM.Pushpin(loc));
        },

        _popPushpin: function () {
            pushpins.pop();
        },

        _createLine: function (point1, point2, withReplace, drawingStyle) {
            logger.log("_createLine");
            var lineVertices = [map.tryPixelToLocation(point1), map.tryPixelToLocation(point2)];
            if (withReplace) polylines.pop();
            polylines.push(new MM.Polyline(lineVertices, (!drawingStyle) ? lineDrawingStyle : drawingStyle));
        },

        _createPolyLine: function (vertices, withReplace, drawingStyle) {
            logger.log("_createPolyLine");
            if (withReplace) {
                polylines.pop();
            }
            var lineVertices = [];
            for (var i = 0; i < vertices.length; i++) {
                var location = map.tryPixelToLocation(vertices[i]);
                lineVertices.push(location);
                logger.log(location);
            }
            polylines.push(new MM.Polyline(lineVertices, (!drawingStyle) ? polyDrawingStyle : drawingStyle));
        },

        _createPolyLineFromLocations: function (locations, withReplace, drawingStyle) {
            logger.log("_createPolyLineFromLocations");
            if (withReplace) {
                polylines.pop();
            }
            polylines.push(new MM.Polyline(locations, (!drawingStyle) ? polyLocationsDrawingStyle : drawingStyle));
        },

        _createShape: function (shapePoints, shapeStyle) {
            logger.log("_createShape");
            var locations = [];
            for (var i = 0; i < shapePoints.length; i++) {
                var loc = map.tryPixelToLocation(shapePoints[i]);
                locations.push(loc);
                logger.log("Point: " + shapePoints[i].x + ", " + shapePoints[i].y);
                logger.log("Loc: " + loc.longitude + ", " + loc.latitude);
            }
            shapes.push(new MM.Polygon(locations, (!shapeStyle) ? polyFillStyle : shapeStyle));
        },

        _createShapeFromLocations: function (locations, shapeStyle) {
            logger.log("_createShapeFromLocations");
            shapes.push(new MM.Polygon(locations, (!shapeStyle) ? polyLocationsFillStyle : shapeStyle));
        },

        _detachEventHandlers: function () {
            logger.log("_detachEventHandlers");
            if (mapClickEventHandler !== null) MM.Events.removeHandler(mapClickEventHandler);
            if (mapMouseDownEventHandler !== null) MM.Events.removeHandler(mapMouseDownEventHandler);
            if (mapMouseUpEventHandler !== null) MM.Events.removeHandler(mapMouseUpEventHandler);
            if (mapMouseMoveEventHandler !== null) MM.Events.removeHandler(mapMouseMoveEventHandler);
        },

        destroy: function () {
            logger.log("destroy");
            $.Widget.prototype.destroy.call(this);
            this.mapDiv.remove();
        }

    });

}(jQuery));