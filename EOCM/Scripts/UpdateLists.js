function UpdateDistricts(GovtObj,DistrictObj,msg,VillageObj,msg2)
{
   
if ((GovtObj).val() != "") {
    var options = {};
    var Govt_ID = (GovtObj).val();
    var dataToSend = {
        Govt_ID: Govt_ID,
    };

    $.ajax({
        url: "/Home/GetDistricts",
        type: "POST",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        async: false,
        data: JSON.stringify(dataToSend),
        success: function (districts) {

           (DistrictObj).empty();
           (DistrictObj).append("<option value= 0 >" + msg + "</option>");
            $.each(districts, function (i, district) {
                (DistrictObj).append('<option value="' + district.Value + '">' +
                     district.Text + '</option>');
            });
            (DistrictObj).prop("disabled", false);
        },
        error: function (ex) {
            alert('خطأ في استعادة اسماء المراكز' + ex);
        }
    });
}
else {
    (DistrictObj).empty();
    (DistrictObj).append("<option value= 0 >" + msg + "</option>");
    (DistrictObj).prop("disabled", true);

    if(arguments.length>3)
    {
        (VillageObj).empty();
        (VillageObj).append("<option value= 0 >" + msg2 + "</option>");
        (VillageObj).prop("disabled", true);
    }
}
}

function UpdateVillages(DistrictObj,VillageObj,msg) {

    if ((DistrictObj).val() != "" && (DistrictObj).val() != "0") {
        var options = {};
        var District_ID = (DistrictObj).val();
        var dataToSend = {
            District_ID: District_ID,
        };

        $.ajax({
            url: "/Home/GetVillages",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false,
            data: JSON.stringify(dataToSend),
            success: function (villages) {

                (VillageObj).empty();
                (VillageObj).append("<option value= 0 >"+msg+"</option>");
                $.each(villages, function (i, village) {
                    (VillageObj).append('<option value="' + village.Value + '">' +
                         village.Text + '</option>');
                });
                (VillageObj).prop("disabled", false);
            },
            error: function (ex) {
                alert('خطأ في استعادة اسماء القرى' + ex);
            }
        });
    }
    else {
        (VillageObj).empty();
        (VillageObj).append("<option value= 0 >" + msg+ "</option>");
        (VillageObj).prop("disabled", true);
    }
}
function UpdateFields(SectorObj, FieldObj,ProductObj,msg) {


    if (SectorObj.val() != "") {
        var options = {};
        var Sector_ID = SectorObj.val();
        var dataToSend = {
            Sector_ID: Sector_ID,
        };

        $.ajax({
            url: "/Home/GetFields",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false,
            data: JSON.stringify(dataToSend),
            success: function (fields) {

                FieldObj.empty();
                FieldObj.append("<option value= 0 >"+ msg+ "</option>");
                $.each(fields, function (i, field) {
                    FieldObj.append('<option value="' + field.Value + '">' +
                         field.Text + '</option>');
                });
                FieldObj.prop("disabled", false);
            },
            error: function (ex) {
                alert('خطأ في استعادة اسماء الانشطة' + ex);
            }
        });
    }
    else {
        FieldObj.empty();
        FieldObj.append("<option value= 0 >"+msg+ "</option>");
        FieldObj.prop("disabled", true);
        ProductObj.empty();
        ProductObj.append("<option value= 0 >"+msg+"</option>");
        ProductObj.prop("disabled", true);
    }
}

function UpdateProducts(FieldObj, ProductObj,msg) 
{

    if (FieldObj.val() != "0" && FieldObj.val() != "") {
        var options = {};
        var Field_ID = FieldObj.val();
        var dataToSend = {
            Field_ID: Field_ID,
        };

        $.ajax({
            url: "/Home/GetProducts",
            type: "POST",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            async: false,
            data: JSON.stringify(dataToSend),
            success: function (products) {

                ProductObj.empty();
                ProductObj.append("<option value= 0 >" + msg+"</option>");
                $.each(products, function (i, product) {
                    ProductObj.append('<option value="' + product.Value + '">' +
                         product.Text + '</option>');
                });
                ProductObj.prop("disabled", false);
            },
            error: function (ex) {
                alert('خطأ في استعادة اسماء المنتجات' + ex);
            }
        });
    }
    else {
        ProductObj.empty();
        ProductObj.append("<option value= 0 >" + msg+"</option>");
        ProductObj.prop("disabled", true);
    }
}

