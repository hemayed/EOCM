

function MultipartialUpdate(views) {
    for (v in views)
        if (views[v].script) {
            eval(views[v].script);
        }
        else {
            $('#' + views[v].updateTargetId).html(views[v].html);
        }
    return false;
}

function UpdateViews(Govt_ID, District_ID, Sector_ID, Field_ID, Product_ID) {

    var dataToSend = {
        Govt_ID: Govt_ID,
        District_ID: District_ID,
        Sector_ID: Sector_ID,
        Field_ID: Field_ID,
        Product_ID: Product_ID,
        ClusterNature_ID: "",
        ClusterType_ID: "",
        IncomeLevel_ID: "",
        ExportFlag_ID: "",
        ProductSeason_ID: ""
    };
    $.ajax({
        url: "/Home/_ClusterMap",
        type: "POST",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dataToSend),
        success: function (result) {
            MultipartialUpdate(result);
        },
    });
}

function UpdateViews2(Govt_ID, District_ID, Sector_ID, Field_ID, Product_ID,ClusterNature_ID,ClusterType_ID,IncomeLevel_ID,ExportFlag_ID,ProductSeason_ID) {

    var dataToSend = {
        Govt_ID: Govt_ID,
        District_ID: District_ID,
        Sector_ID: Sector_ID,
        Field_ID: Field_ID,
        Product_ID: Product_ID,
        ClusterNature_ID: ClusterNature_ID,
        ClusterType_ID: ClusterType_ID,
        IncomeLevel_ID: IncomeLevel_ID,
        ExportFlag_ID: ExportFlag_ID,
        ProductSeason_ID:ProductSeason_ID
    };
    $.ajax({
        url: "/Home/_ClusterMap",
        type: "POST",
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(dataToSend),
        success: function (result) {
            MultipartialUpdate(result);
        },
    });
}


