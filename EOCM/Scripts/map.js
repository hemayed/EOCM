
var map=null, infobox, dataLayer;


function GetMap(myArray) {
    // Initialize the map

    var num = myArray.length;
  
    var clat = 0;
    var clon = 0;

    for (var i = 0; i < num; i++)
    {
        clat = clat + myArray[i].Cluster_Lat;
        clon = clon + myArray[i].Cluster_Long;
    }

    var zoomLevel = 7;

    if (num > 0)
    {
        clat = clat / num;
        clon = clon / num;
    }
    else
    {
        clat = 30.1 ;
        clon = 31.26;
    }
    
   
//    try{
//        map = new Microsoft.Maps.Map(document.getElementById("myMap"),
//               {
//                   credentials: "ApsJjM2R2v3U-bnatAF3H0IY4cbas9KnKtIwKzOsLVICG3kqmJaDUZEh_8J-RzR7",
//                   center: new Microsoft.Maps.Location(clat, clon),
//                   mapTypeId: Microsoft.Maps.MapTypeId.road,
//                   zoom: zoomLevel
//               });
//    }
//    catch (e) {
//        alert("الخريطة غير متاحة الان - حاول لاحقا");
//    }
     
  
//if (num>0 && map !=null)
//    {
//    dataLayer = new Microsoft.Maps.EntityCollection();
//    map.entities.push(dataLayer);

//    var infoboxLayer = new Microsoft.Maps.EntityCollection();
//    map.entities.push(infoboxLayer);

//    infobox = new Microsoft.Maps.Infobox(new Microsoft.Maps.Location(0, 0), { visible: false, offset: new Microsoft.Maps.Point(0, 20) });
//    infoboxLayer.push(infobox);

    
//        AddData(myArray);
//    }
        
}




function AddData(myArray) {
   
    var pin = [300];
    var pushpinOptions;
    var num = myArray.length;
    var refSection="";
    var imgSection = "";

    for (i = 0; i < num; i++) {

        var pintxt = String(i + 1);
        pushpinOptions = { text: pintxt, visible: true };
        var location1 = new Microsoft.Maps.Location(myArray[i].Cluster_Lat, myArray[i].Cluster_Long);
        pin[i] = new Microsoft.Maps.Pushpin(location1,pushpinOptions);
        pin[i].Title = myArray[i].Cluster_Name;
        
        pin[i].Description = myArray[i].Cluster_Info1 + "<br>" + myArray[i].Cluster_Info2;
        pin[i].showCloseButton = true;
        //pin[i].titleClickHandler = titleClick(info1[i]);
       
        if (myArray[i].Cluster_DetailPage != null && myArray[i].Cluster_DetailPage != "")
        {
            refSection = '<a href="' + myArray[i].Cluster_DetailPage + '" target="_blank">';
        }
        else
        {
            refSection = '';
        }

        if (myArray[i].Cluster_ProductImage != null && myArray[i].Cluster_ProductImage != "") {
            imgSection = '<img src= "' + myArray[i].Cluster_ProductImage + '" alt="Product Image" style="position:absolute; top:5px; left:5px; width:65px; height:65px">';
        }
        else {
            imgSection = '';
        }

        pin[i].htmlContent = '<div id="infoboxText' + myArray[i].Cluster_Num + '" style="direction: rtl; background-color:White; border-style:solid;border-width:medium; border-color:DarkOrange; min-height:120px;width:250px;">' + refSection + ' <b id="infoboxTitle' + i + '" style="text-decoration:underline; position:absolute; top:10px; left:10px; width:220px;"> ' + myArray[i].Cluster_Name + '</b> </a> <a id="infoboxDescription' + i + '" style="text-decoration:none; color:#000000; position:absolute; top:30px; left:10px; width:220px;">' + myArray[i].Cluster_Info1 + "<br>" + myArray[i].Cluster_Info2 + "<br>" + myArray[i].Cluster_Info3 + '</a>' + imgSection + '</div>';


        // Add handler for the pushpin click event.
        Microsoft.Maps.Events.addHandler(pin[i], 'click', displayInfobox);

        dataLayer.push(pin[i]);
        

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