
var map=null, infobox, dataLayer;
var mapWidth=500;
var mapHeight=460;

function GetMap(myArray) {
    // Initialize the map
    //default zoom scales in km/pixel from http://msdn2.microsoft.com/en-us/library/aa940990.aspx
   var defaultScales = [ 78.27152, 39.13576, 19.56788, 9.78394, 4.89197, 2.44598, 1.22299, 
0.61150, 0.30575, 0.15287, .07644, 0.03822, 0.01911, 0.00955, 0.00478, 0.00239, 0.00119, 0.0006, 0.0003 ];

    var num = myArray.length;
  
    var cLat = 0;
    var cLon = 0;
    var maxLat=-90;
    var minLat = 90;
    var maxLon = -180;
    var minLon = 180;

    //var minLowerWidth=0;
    //var maxLowerWidth=50;
    //var minUpperWidth=450;
    //var maxUpperWidth=500;

    for (var i = 0; i < num; i++)
    {
        cLat = cLat + myArray[i].Cluster_Lat;
        cLon = cLon + myArray[i].Cluster_Long;

        if (myArray[i].Cluster_Lat>maxLat)
            maxLat=myArray[i].Cluster_Lat;
        if (myArray[i].Cluster_Lat<minLat)
            minLat = myArray[i].Cluster_Lat;

        if (myArray[i].Cluster_Long > maxLon)
            maxLon = myArray[i].Cluster_Long;
        if (myArray[i].Cluster_Long < minLon)
            minLon = myArray[i].Cluster_Long;
       
    }

    //calculate center coordinate of bounding box
     cLat = (maxLat + minLat) / 2;
     cLon = (maxLon + minLon) / 2;

    //want to calculate the distance in km along the center latitude between the two longitudes
     var meanDistanceX = HaversineDistance(cLat, minLon, cLat, maxLon);

    //want to calculate the distance in km along the center longitude between the two latitudes
     var meanDistanceY = HaversineDistance(maxLat, cLon, minLat, cLon) * 2;

    //calculates the x and y scales
     var meanScaleValueX = meanDistanceX / mapWidth;
     var meanScaleValueY = meanDistanceY / mapHeight;

     var meanScale;

    //gets the largest scale value to work with
     if (meanScaleValueX > meanScaleValueY)
         meanScale = meanScaleValueX;
     else
         meanScale = meanScaleValueY;

    //intialize zoom level variable
     var zoomLevel = 1;

    //calculate zoom level
     for (var i = 1; i < 19; i++)
     {
         if (meanScale >= defaultScales[i])
         {
             zoomLevel = i;
             break;
         }
     }


    if (num == 0) {
        
        cLat = 30.1;
        cLon = 31.26;
        zoomLevel = 7;
    }
  
    if (num == 1) {

        zoomLevel = 9;
    }

    try{
        map = new Microsoft.Maps.Map(document.getElementById("myMap"),
               {
                   credentials: "ApsJjM2R2v3U-bnatAF3H0IY4cbas9KnKtIwKzOsLVICG3kqmJaDUZEh_8J-RzR7",
                   center: new Microsoft.Maps.Location(cLat, cLon),
                   mapTypeId: Microsoft.Maps.MapTypeId.road,
                   zoom: zoomLevel
               });
    }
    catch (e) {
        alert("الخريطة غير متاحة الان - حاول لاحقا");
    }
     
  
if (num>0 && map !=null)
    {
    dataLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(dataLayer);

    var infoboxLayer = new Microsoft.Maps.EntityCollection();
    map.entities.push(infoboxLayer);

    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 20) });
    infoboxLayer.push(infobox);

    
        AddData(myArray);
    }
        
}

/// <summary>
/// Calculate the distance in kilometers between two coordinates
/// </summary>
/// <param name="lat1"></param>
/// <param name="lon1"></param>
/// <param name="lat2"></param>
/// <param name="lon2"></param>
/// <returns></returns>
function HaversineDistance(lat1, lon1, lat2, lon2)
{
    var earthRadius = 6371;
    var factor = Math.PI / 180;
    var dLat = (lat2 - lat1) * factor;
    var dLon = (lon2 - lon1) * factor;
    var a = Math.sin(dLat / 2) * Math.sin(dLat / 2) + Math.cos(lat1 * factor) * Math.cos(lat2 * factor) 
* Math.sin(dLon / 2) * Math.sin(dLon / 2);

    var c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

return earthRadius * c;
}


function AddData(myArray) {
   
    var pin = [300];
    var pushpinOptions;
    var num = myArray.length;
    var refSection="";
    var imgSection = "";

   

    for (i = 0; i < num; i++) {

        var pintxt = String(i + 1);
        
        switch (myArray[i].Sector_ID) {

            case 1: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/RedPushPin.png", visible: true };
                break;
            case 2: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/GreenPushPin.png", visible: true };
                break;
            case 3: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/LightBluePushPin.png", visible: true };
                break;
            case 4: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/BlackPushPin.png", visible: true };
                break;
            case 5: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/YellowPushPin.png", visible: true };
                break;
            default: pushpinOptions = { typeName: 'pin' + pintxt, text: pintxt, icon: "Images/TransparentPushPin.png", visible: true };
                break;
        }

        var location1 = new Microsoft.Maps.Location(myArray[i].Cluster_Lat, myArray[i].Cluster_Long);
        pin[i] = new Microsoft.Maps.Pushpin(location1,pushpinOptions);
        pin[i].Title = myArray[i].Cluster_Name;

       
      

      
      

        
        pin[i].Description = myArray[i].Cluster_Info1 + "<br>" + myArray[i].Cluster_Info2;
        pin[i].showCloseButton = true;
        //pin[i].titleClickHandler = titleClick(info1[i]);
       
       
          
        //refSection1 = '@Ajax.ActionLink('+myArray[i].Cluster_Name + ', "ClusterDetail", new { id =' + myArray[i].Cluster_ID + '}, new AjaxOptions() { HttpMethod = "Post" }, new { target = "_blank" })';
        //refSection2 = '';

        //if (myArray[i].Cluster_DetailPage != null && myArray[i].Cluster_DetailPage != "")
        //{
        //    refSection1 = '<a href="' + myArray[i].Cluster_DetailPage + '" target="_blank">';
        //    refSection2 = '</a>';
        //}
        //else
        //{
        //    refSection1 = '';
        //    refSection2 = '';
        //}

            refSection1 = '';
            refSection2 = '';


        if (myArray[i].Cluster_ProductImage != null && myArray[i].Cluster_ProductImage != "") {
            imgSection = '<img src= "../' + myArray[i].Cluster_ProductImage + '" alt="Product Image" style="position:absolute; top:40px; left:1px; width:40px; height:40px">';
        }
        else {
            imgSection = '';
        }
        var divID="infoboxText"+myArray[i].Cluster_Num;
        
        pin[i].htmlContent = '<div id="' + divID + '" style="direction: rtl; background-color:White; border-style:solid;border-width:medium; border-color:DarkOrange; position:relative; top:-12px; left:-100px; min-height:145px;width:200px; ">' + 
            '<button class="close" style="text-decoration:none; position:absolute; top:1px; left:1px;" onclick="document.getElementById(\'' + divID + '\').style.display =\'none\'">X</button>' +
            refSection1 +
            '<b id="infoboxTitle' + i +
            '" style="text-decoration:underline; position:absolute; top:0px; right:1px; width:180px;"> ' + myArray[i].Cluster_Num + '-' + myArray[i].Cluster_Name + refSection2 +
            '</b> <a id="infoboxDescription' + i + '" style="text-decoration:none; color:#000000; position:relative; top:18px; right:1px; min-height:50; width:198px;">' + myArray[i].Cluster_Info1 + "<br>" + myArray[i].Cluster_Info2 + "<br>" + myArray[i].Cluster_Info3 + "<br>" + myArray[i].Cluster_Info4 + '</a>' + imgSection + '</div>';

      
        // Add handler for the pushpin click event.
        Microsoft.Maps.Events.addHandler(pin[i], 'click', displayInfobox);

     

        dataLayer.push(pin[i]);

        //switch (myArray[i].Sector_ID) {

        //    case 1: $('.pin' + pintxt + ' div').css({ 'background-color': 'red' }); break;
        //    case 2: $('.pin' + pintxt + ' div').css({ 'background-color': 'green' }); break;
        //    case 3: $('.pin' + pintxt + ' div').css({ 'background-color': 'blue' }); break;
        //    case 4: $('.pin' + pintxt + ' div').css({ 'background-color': 'yellow' }); break;
        //    case 5: $('.pin' + pintxt + ' div').css({ 'background-color': 'pink' }); break;
        //    default: $('.pin' + pintxt + ' div').css({ 'background-color': 'orange' }); 

        //}
       
      

    }
}
function displayInfobox(e) {
    if (e.targetType == 'pushpin') {
        infobox.setLocation(e.target.getLocation());
        //infobox.setOptions({ visible: true, title: e.target.Title, description: e.target.Description });
        infobox.setOptions({ visible: true, htmlContent: e.target.htmlContent });
    }
}

function closeInfoBox() {
    var infobox = document.getElementById('infoBox');
    infoBox.style.visibility = "hidden";
}

 function titleClick(title) {
            alert("I will open a new page to display detailed info about "+  title);
 }


 function createCanvasPins(lat, lon, sectorID) {
     var pin, img;

         //Create a canvas pushpin at a random location
         pin = new CanvasPushpin(new Microsoft.Maps.Location(lat,lon), function (pin, context) {
             img = new Image();
             img.onload = function () {
                 if (context) {
                     //Set the dimensions of the canvas
                     context.width = img.width;
                     context.height = img.height;

                     //Draw a colored circle behind the pin
                     context.beginPath();
                     context.arc(13, 13, 11, 0, 2 * Math.PI, false);
                     context.fillStyle = pin.Metadata.color;
                     context.fill();

                     //Draw the pushpin icon
                     context.drawImage(img, 0, 0);
                 }
             };

             img.src = 'Images/TransparentPushPin.png';
         });

         //Give the pushpin a random color
         pin.Metadata = {
             color: generateRandomColor()
         };

         //Add the pushpin to the Canvas Entity Collection
         canvasLayer.push(pin);
 }

 function generateRandomColor(sectorID) {
     switch (sectorID) {
         case 1: return rgb(100, 0, 0); break;
         case 2: return rgb(0, 100, 0); break;
         case 3: return rgb(0, 0, 100); break;
         case 4: return rgb(100, 100, 0); break;
         default: return rgb(100, 0, 100); break;

     }
 }
