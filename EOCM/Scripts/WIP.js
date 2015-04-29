
//    $(document).ready(function () {

//        $.fn.dataTableExt.oPagination.paging_with_jqui_icons = {
//            "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
//                var nFirst = document.createElement('span');
//                var nPrevious = document.createElement('span');
//                var nNext = document.createElement('span');
//                var nLast = document.createElement('span');

//                nFirst.innerHTML = "";
//                nPrevious.innerHTML = "";
//                nNext.innerHTML = "";
//                nLast.innerHTML = "";

//                nFirst.className = "ui-icon ui-icon-seek-first first";
//                nFirst.style.display = "inline-block";
//                nPrevious.className = "ui-icon ui-icon-seek-prev previous";
//                nPrevious.style.display = "inline-block";
//                nNext.className = "ui-icon ui-icon-seek-next next";
//                nNext.style.display = "inline-block";
//                nLast.className = "ui-icon ui-icon-seek-end last";
//                nLast.style.display = "inline-block";

//                if (oSettings.sTableId !== '') {
//                    nPaging.setAttribute('id', oSettings.sTableId + '_paginate');
//                    nFirst.setAttribute('id', oSettings.sTableId + '_first');
//                    nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
//                    nNext.setAttribute('id', oSettings.sTableId + '_next');
//                    nLast.setAttribute('id', oSettings.sTableId + '_last');
//                }

//                nPaging.appendChild(nFirst);
//                nPaging.appendChild(nPrevious);
//                nPaging.appendChild(nNext);
//                nPaging.appendChild(nLast);

//                $(nFirst).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "first");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nPrevious).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "previous");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nNext).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "next");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nLast).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "last");
//                    fnCallbackDraw(oSettings);
//                });
//            },

//            "fnUpdate": function (oSettings, fnCallbackDraw) {
//                if (!oSettings.aanFeatures.p)
//                    return;

//                /* Loop over each instance of the pager. */
//                var an = oSettings.aanFeatures.p;

//                for (var i = 0, iLen = an.length; i < iLen; i++) {
//                    var icons = an[i].getElementsByTagName('span');

//                    if (oSettings._iDisplayStart === 0) {
//                        icons[0].style.display = "none";
//                        icons[1].style.display = "none";
//                    }
//                    else {
//                        icons[0].style.display = "inline-block";
//                        icons[1].style.display = "inline-block";
//                    }

//                    if (oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay()) {
//                        icons[2].style.display = "none";
//                        icons[3].style.display = "none";
//                    }
//                    else {
//                        icons[2].style.display = "inline-block";
//                        icons[3].style.display = "inline-block";
//                    }
//                }
//            }
//        };

//        $('#clusterTable').DataTable({
//            //"dom": '<"top"i>rt<"bottom"flp><"clear">',
//            //    "columns": [{ "width": "20%" }, null],
//            "sPaginationType": "paging_with_jqui_icons",
//            //"oLanguage": {
//            //    "sSearch": "بحث",
//            //    "sEmptyTable": "لايوجد تجمعات",
//            //    "sZeroRecords": "لايوجد تجمعات",
//            //    "sInfo": "_TOTAL_ من _END_ الى _START_",
//            //    "sInfoFiltered": "",
//            //    "sLengthMenu": ' <select>' +
//            //    '<option value="10">10</option>' +
//            //    '<option value="20">20</option>' +
//            //    '<option value="30">30</option>' +
//            //    '<option value="40">40</option>' +
//            //    '<option value="50">50</option>' +
//            //    '<option value="-1">الكل</option>' +
//            //    '</select>عرض',
//            //    "oPaginate": {
//            //        "sFirst": "",
//            //        "sPrevious": "",
//            //        "sNext": "",
//            //        "sLast": ""
//            //    }
//            //}
//        });



//    });

//</script>*@

//@*<script>
//    $(document).ready(function () {

//        $.fn.dataTableExt.oPagination.four_button = {
//            /*
//             * Function: oPagination.four_button.fnInit
//             * Purpose:  Initalise dom elements required for pagination with a list of the pages
//             * Returns:  -
//             * Inputs:   object:oSettings - dataTables settings object
//             *           node:nPaging - the DIV which contains this pagination control
//             *           function:fnCallbackDraw - draw function which must be called on update
//             */
//            "fnInit": function (oSettings, nPaging, fnCallbackDraw) {
//                nFirst = document.createElement('span');
//                nPrevious = document.createElement('span');
//                nNext = document.createElement('span');
//                nLast = document.createElement('span');

//                nFirst.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sFirst));
//                nPrevious.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sPrevious));
//                nNext.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sNext));
//                nLast.appendChild(document.createTextNode(oSettings.oLanguage.oPaginate.sLast));

//                //nFirst.className = "paginate_button first";
//                //nPrevious.className = "paginate_button previous";
//                //nNext.className = "paginate_button next";
//                //nLast.className = "paginate_button last";

//                nFirst.innerHTML = "";
//                nPrevious.innerHTML = "";
//                nNext.innerHTML = "";
//                nLast.innerHTML = "";

//                nFirst.className = "ui-icon ui-icon-seek-start first";
//                nFirst.style.display = "inline-block";
//                nPrevious.className = "ui-icon ui-icon-seek-prev previous";
//                nPrevious.style.display = "inline-block";
//                nNext.className = "ui-icon ui-icon-seek-next next";
//                nNext.style.display = "inline-block";
//                nLast.className = "ui-icon ui-icon-seek-end last";
//                nLast.style.display = "inline-block";

//                //if (oSettings.sTableId !== '') {
//                nPaging.setAttribute('id', oSettings.sTableId + '_paginate');
//                nFirst.setAttribute('id', oSettings.sTableId + '_first');
//                nPrevious.setAttribute('id', oSettings.sTableId + '_previous');
//                nNext.setAttribute('id', oSettings.sTableId + '_next');
//                nLast.setAttribute('id', oSettings.sTableId + '_last');
//                //}

//                nPaging.appendChild(nFirst);
//                nPaging.appendChild(nPrevious);
//                nPaging.appendChild(nNext);
//                nPaging.appendChild(nLast);

//                $(nFirst).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "first");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nPrevious).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "previous");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nNext).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "next");
//                    fnCallbackDraw(oSettings);
//                });

//                $(nLast).click(function () {
//                    oSettings.oApi._fnPageChange(oSettings, "last");
//                    fnCallbackDraw(oSettings);
//                });

//                /* Disallow text selection */
//                $(nFirst).bind('selectstart', function () { return false; });
//                $(nPrevious).bind('selectstart', function () { return false; });
//                $(nNext).bind('selectstart', function () { return false; });
//                $(nLast).bind('selectstart', function () { return false; });
//            },

//            /*
//             * Function: oPagination.four_button.fnUpdate
//             * Purpose:  Update the list of page buttons shows
//             * Returns:  -
//             * Inputs:   object:oSettings - dataTables settings object
//             *           function:fnCallbackDraw - draw function which must be called on update
//             */
//            "fnUpdate": function (oSettings, fnCallbackDraw) {
//                if (!oSettings.aanFeatures.p) {
//                    return;
//                }

//                /* Loop over each instance of the pager */
//                var an = oSettings.aanFeatures.p;
//                var iLen = an.length;
//                for (var i = 0 ; i < iLen ; i++) {
//                    var buttons = an[i].getElementsByTagName('span');
//                    if (oSettings._idisplaystart === 0) {
//                        buttons[0].classname = "paginate_disabled_previous";
//                        buttons[1].classname = "paginate_disabled_previous";
//                    }
//                    else {
//                        buttons[0].classname = "paginate_enabled_previous";
//                        buttons[1].classname = "paginate_enabled_previous";
//                    }
//                    if (oSettings.fnDisplayEnd() == oSettings.fnRecordsDisplay()) {
//                        buttons[2].classname = "paginate_disabled_next";
//                        buttons[3].classname = "paginate_disabled_next";
//                    }
//                    else {
//                        buttons[2].classname = "paginate_enabled_next";
//                        buttons[3].classname = "paginate_enabled_next";
//                    }
//                }
//            }
//        }; />* Example initialisation */
//        $(document).ready(function () {
//            $('#clusterTable').dataTable({
//                "sPaginationType": "four_button"
//            });
//        });
//    });

