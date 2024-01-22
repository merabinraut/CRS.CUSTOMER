function isAlphabetNum(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 45 || charCode == 47 || (charCode > 47 && charCode < 58)) {
        return true;
    }
    return false;
}
if ($('.datatable').length > 0) {
    $(".datatable").dataTable({
        language: {
            search: "<span>Filter:</span> _INPUT_",
            searchPlaceholder: "Type to filter...",
            lengthMenu: "<span>Show:</span> _MENU_",
            paginate: { "first": "First", "last": "Last", "next": $("html").attr("dir") == "rtl" ? "&larr;" : "&rarr;", "previous": $("html").attr("dir") == "rtl" ? "&rarr;" : "&larr;" }
        },
        "autoWidth": false
    });
}
if ($('.datatable-report').length > 0) {
    $(".datatable-report").dataTable({
        language: {
            search: "<span>Filter:</span> _INPUT_",
            searchPlaceholder: "Type to filter...",
            lengthMenu: "<span>Show:</span> _MENU_",
            paginate: { "first": "First", "last": "Last", "next": $("html").attr("dir") == "rtl" ? "&larr;" : "&rarr;", "previous": $("html").attr("dir") == "rtl" ? "&rarr;" : "&larr;" }
        },
        "autoWidth": false,
        "scrollX": true
    });
}
//Noty.overrideDefaults({

//});
//function ShowPopup(Code, Message) {
//    type = '';
//    if (Code === 1) {
//        type = 'error';
//    }
//    else if (Code === 0) {
//        type = 'success';
//    }
//    else if (Code === 2) {
//        type = 'warning';
//    }
//    new Noty({
//        text: Message,
//        type: type,
//        theme: "limitless",
//        layout: "topRight",
//        timeout: 2500
//    }).show();
//}

//Noty.overrideDefaults({
//    theme: "limitless",
//    layout: "topRight",
//    type: "alert",
//    timeout: 2500
//});

//function ShowPopup(Code, Message) {
//    type = '';
//    if (Code == 1) {
//        type = 'error';
//    }
//    else if (Code == 0) {
//        type = 'success';
//    }
//    else if (Code == 2) {
//        type = 'warning';
//    }
//    new Noty({
//        text: Message,
//        type: type
//    }).show();
//}
$('.select2').select2()

$(document).ready(function e() {
    if (!$().uniform) {
        console.warn('Warning - uniform.min.js is not loaded.');
        return;
    }
    if ($('.form-check-input-styled').length > 0) {
        $('.form-check-input-styled').uniform();
    }
    if (!$().bootstrapSwitch) {
        console.warn('Warning - switch.min.js is not loaded.');
        return;
    }
    if ($('.form-check-input-switch').length > 0) {
        $('.form-check-input-switch').bootstrapSwitch();
    }
    if (typeof (Switchery) == 'undefined') {
        console.warn('Warning - switchery.min.js is not loaded.');
        return;
    }
    if ($('.form-check-input-switchery').length > 0) {

        var elems = Array.prototype.slice.call(document.querySelectorAll('.form-check-input-switchery'));
        elems.forEach(function (html) {
            var switchery = new Switchery(html);
        });
        // Colored switches
        var primary = document.querySelector('.form-check-input-switchery-primary');
        var switcheryPrimary = new Switchery(primary, { color: '#2196F3' });

        var danger = document.querySelector('.form-check-input-switchery-danger');
        var switcheryDanger = new Switchery(danger, { color: '#EF5350' });

        var warning = document.querySelector('.form-check-input-switchery-warning');
        var switcheryWarning = new Switchery(warning, { color: '#FF7043' });

        var info = document.querySelector('.form-check-input-switchery-info');
        var switcheryInfo = new Switchery(info, { color: '#00BCD4' });
    }
});
///urls must begin with ~/
var urls = {
    BlockUser: '~/User/BlockUser',
    UnBlockUser: '~/User/UnBlockUser'
};
let ShowModal = new Function('modalId', 'if($("#"+modalId).length>0){$("#"+modalId).modal("show");}');
let HideModal = new Function('modalId', 'if($("#"+modalId).length>0){$("#"+modalId).modal("hide");}');
let SingleFieldModal = new Function('modalId', 'modalHeader', 'fieldId', 'fieldVal', 'formName', 'formAction', 'modalContent',
    'if($("#"+modalId).length>0 && $("#"+fieldId).length>0)' +
    '{' +
    '$("#"+modalId+" .modal-title").html(modalHeader);$("#"+modalId).modal("show");$("#"+fieldId).val(fieldVal);' +
    '$("#"+formName).attr("action",resolveurl(urls[formAction]));if($("#modalContentToDisplay").length>0){$("#modalContentToDisplay").html(modalContent);}' +
    '}');
let CallAjaxDbResponse = new Function('jsonData', 'url', 'method', 'redirect', 'evalText=""',
    '$.ajax({' +
    'url: resolveurl(url),' +
    'type: method,' +
    'data: jsonData,' +
    //'contentType: "contentType",' +
    'error: function () {' +
    //'ShowPopup(1, "Something Went Wrong.");' +
    '},' +
    'success: function (response) {' +
    'if(redirect==true){window.location.reload();} ;if (response.Code == 0) {' +
    'eval(evalText);' +
    '};if (response.isRedirect) {' +
    'window.location.href = response.redirectUrl;' +
    '};if(response.isRedirect==false){' +
    'window.location.reload();' +
    '}' +
    'else {' +
    //'ShowPopupModel(response.Code, response.Message);' +
    '}' +
    '}' +
    '});');
function showConfirmationModal(message, funcText, title = "Are you sure?") {

    bootbox.confirm({
        title: title,
        message: message,
        icon: "icon-remove",
        buttons: {
            confirm: {
                label: 'Yes',
                classname: 'btn-primary'
            },
            cancel: {
                label: 'No',
                classname: 'btn-link'
            }
        },
        callback: function (result) {
            if (result) {
                eval(funcText);
            }
            return;
        }
    });
}



function ShowPopupModel(Code, Message, Title, Extra1 = "false") {
    MyModelType = '';
    MyModelContent = '';
    var Icon = "";
    var titleAttach = "";
    if (Code == 0) {
        MyModelType = 'modal-success';
        MyModelContent = 'bg-success';
        titleAttach = "Success";
        Icon = "fas fa-check";
    }
    else if (Code == 1) {
        MyModelType = 'modal-danger';
        MyModelContent = 'bg-danger';
        titleAttach = "Failed";
        Icon = "fas fa-times";

    }
    else if (Code == 2) {
        MyModelType = 'modal-warning';
        MyModelContent = 'bg-warning';
        titleAttach = "Warning";
        Icon = "fas fa-exclamation";

    }
    else if (Code == 9) {
        MyModelType = 'modal-info';
        MyModelContent = 'bg-info';
        titleAttach = "Exception";
        Icon = "fas fa-info";

    }
    console.log(typeof (Title));
    if (Title == "" || Title == "0" || Title == "1" || Title == "2" || Title == "9" || Title == undefined) {
        Title = titleAttach.toString();
    }
    $(".MyModelType").removeAttr("id");
    $(".MyModelType").attr("id", MyModelType);
    if (Extra1 === "false" || Extra1 === "") {
        $(".MyModelFooterClose").removeAttr("onClick");
        $(".MyCloseIcon").removeAttr("onClick");

    } else {
        $(".MyModelFooterClose").attr("onClick", "window.location.reload();");
        $(".MyCloseIcon").attr("onClick", "window.location.reload();");
    }
    $(".modal-header").addClass(MyModelContent);
    $(".MyModelTitle").empty().prepend("<i class='" + Icon + "'></i> &nbsp;&nbsp;&nbsp;&nbsp;" + titleAttach);
    $(".MyModelBody").empty().prepend(Message);
    $('.MyModelType').modal('show');
}

let DataTableWithExportToExcelTotal = new Function('className', 'getTotalOfColumns', 'footerdisplay', 'coulmnvisible', 'commaCols', 'printCol',
    'if ($("."+className).length > 0) {' +
    ' var numberRenderer = $.fn.dataTable.render.number(",", ".", 2,).display;' +
    '$("."+className).dataTable({' +
    'dom: "Blfrtip",' +
    '"lengthMenu": [[10 ,25, 50,100],[10, 25, 50,100]],' +
    '"columnDefs": [{' +
    '"orderable": false,' +
    '"render": $.fn.dataTable.render.number(",", ".", 2, "",""),' +
    '"targets": commaCols,' +
    '}],' +
    'buttons: [{ extend: "excelHtml5",' +
    'title: document.title + moment().format(" YYYY MM DD h: mm: ss a"),' +

    'customize: function(xlsx) {' +
    'var styles = xlsx.xl["styles.xml"];' +
    'var style = \'<xf numFmtId="49" fontId="0" fillId="0" borderId="0" xfId="0" applyFont="1" applyFill="1" applyBorder="1" applyNumberFormat="1"/>\';' +
    'var el = $("cellXfs", styles);' +
    'el.append(style).attr("count", parseInt(el.attr("count"))+1);' +
    'var styleIdx = $("xf", el).length - 1;' +
    'var sheet = xlsx.xl.worksheets["sheet1.xml"];' +
    '$("col", sheet).attr("style", styleIdx);' +
    '$("row:gt(0) c[r^=A]", sheet).attr("s", styleIdx);' +
    ' },' +
    'exportOptions: { ' +
    'format: {' +

    'body: function (data, row, column, node) {' +
    'var root=document.createElement("div");root.innerHTML=data;' +
    'return "\0" + root.innerText   ;' +
    ' }' +
    '},' +
    'columns: \':visible:not(.colAction)\'},' +
    'footer: footerdisplay },' +
    '{' +
    'extend: "colvis",' +
    'text: "Show/hide columns"' +
    '}' +
    //',{' +
    //'extend: "pdfHtml5", orientation: "landscape", landscape: "LEGAL",' +
    //'text: "PDF"' +
    ////',customize:function(doc){doc.content[0].table.widths=Array(doc.content[1].table.body[0].length+1).join("*").split("");}'+
    ////',customize : function(doc){var colCount = new Array();$("table").find("tbody tr:first-child td").each(function(){if($(this).attr("colspan")){for(var i=1;i<=$(this).attr("colspan");$i++){colCount.push("*");}}else{colCount.push("*");}});doc.content[1].table.widths = colCount;}'+
    //'}' +

    ' ], ' +
    'language: {' +
    '"decimal": ".",' +
    '"thousands": ",",' +
    'search: "<span>Filter:</span> _INPUT_",' +
    'searchPlaceholder: "Type to filter...",' +
    'paginate: {' +
    '"first": "First", "last": "Last", "next": $("html").attr("dir") == "rtl" ? "&larr;" : "&rarr;", "previous": $("html").attr("dir") == "rtl" ? "&rarr; " : "&larr; "' +
    '}' +
    '},' +
    '"autoWidth": false,' +

    '"footerCallback": function (row, data, start, end, display) {' +
    'var api = this.api(), data;' +
    'var intVal = function (i) {' +
    'return typeof i === "string" ?' +
    'i.replace(/[\$,]/g, "") * 1 :' +
    'typeof i === "number" ?' +
    'i : 0;' +
    '};' +
    'for (let j = 0; j < getTotalOfColumns.length; j++) {' +
    'numberRenderer(getTotalOfColumns);' +
    'total = api' +
    '.column(getTotalOfColumns[j])' +
    '.data()' +
    '.reduce(function (a, b) {' +
    'return intVal(a) + intVal(b);' +
    '}, 0);' +
    'pageTotal = api' +
    '.column(getTotalOfColumns[j], { page: "current" })' +
    '.data()' +
    '.reduce(function (a, b) {' +
    'var ptotal = intVal(a) + intVal(b);' +
    'return ptotal.toFixed(3);' +
    '}, 0);' +
    '$(api.column(getTotalOfColumns[j]).footer()).html(' +
    'numberRenderer(pageTotal) + " (" + numberRenderer(total.toFixed(3)) + " Grand total)"' +
    ');' +
    '}' +
    '}' +

    '});' +
    '}')






function showDateTableWithExcelTotal(className, colArray, footerdisplay, columnvisible, commaColArray) {

    var columns = "";
    let visiblecols = "";
    let commaCols = "";
    for (let k = 0; k < colArray.length; k++) {
        if (k >= colArray.length - 1) {
            columns += colArray[k];
        }
        else
            columns += colArray[k] + ","
    }

    for (let j = 0; j < commaColArray.length; j++) {
        if (j >= commaColArray.length - 1) {
            commaCols += commaColArray[j];
        }
        else
            commaCols += commaColArray[j] + ","
    }

    for (let k = 0; k < columnvisible.length; k++) {
        if (k >= columnvisible.length - 1) {
            visiblecols += columnvisible[k];
        }
        else
            visiblecols += columnvisible[k] + ","
    }
    eval("DataTableWithExportToExcelTotal('" + className + "',[" + columns + "] ," + footerdisplay + ",[" + visiblecols + "],[" + commaCols + "]);");

}

function showFilter() {
    let buttonStatus = document.getElementById("searchToggle");
    let filterCard = document.getElementById("searchsection");
    if (filterCard.style.display == "none") {
        filterCard.style.display = "block";
        buttonStatus.innerHTML = "Hide Filters";
    }
    else if (filterCard.style.display == "block") {
        filterCard.style.display = "none";
        buttonStatus.innerHTML = "Show Filters";
    }
    $('.select2').select2();

}

//Auto Capitalize First letter of input name
//use class Name "names_capitalize"

$('.names_capitalize').on('blur', function (evt) {
    // Get an array of all the words (in all lower case)
    var words = evt.target.value.toLowerCase().split(/\s+/g);

    // Loop through the array and replace the first letter with a captital letter
    var newWords = words.map(function (element) {
        // As long as we're not dealing with an empty array element, return the first letter
        // of the word, converted to upper case and add the rest of the letters from this word.
        // Return the final word to a new array
        return element !== "" ? element[0].toUpperCase() + element.substr(1, element.length) : "";
    });

    // Replace the original value with the updated array of capitalized words.
    evt.target.value = newWords.join(" ");

});

//Prevents PASTING in text/password/email [TYPE]

/*$('input[type=text]').on('paste', e => e.preventDefault());*/
$('input[type=password]').on('paste', e => e.preventDefault());
/*$('input[type=email]').on('paste', e => e.preventDefault());*/

$('.imgValidate').on('input', (e) => {
    input = e.target;
    if (input.files && input.files[0]) {
        var validExtensions = ['jpg', 'png', 'jpeg']; //array of valid extensions
        var fileName = input.files[0].name;
        var fileNameExt = fileName.substr(fileName.lastIndexOf('.') + 1);
        var size = input.files[0].size;
        var imgSizeInMB = size / (1000000);

        //checking for image file extension 
        if ($.inArray(fileNameExt, validExtensions) == -1) {
            $('.modal-body').empty();
            $('.modal-body').prepend("Image must be in jpeg, jpg or png format");
            $("#confirmModalLabel").html("<span class='text-danger'> Invalid image </span>");
            $("#closeModal").html('Close');
            $("#confirmBUtton").hide();
            $("#confirmModal").modal('show');
            e.target.value = "";
        }
        //checking for image file that is greater than 2MB 
        if (imgSizeInMB > 2) {
            $('.modal-body').empty();
            $('.modal-body').prepend("Image size must be less than 2MB");
            $("#confirmModalLabel").html("<span class='text-danger'> Invalid image </span>");
            $("#closeModal").html('Close');
            $("#confirmBUtton").hide();
            $("#confirmModal").modal('show');
            e.target.value = "";
        }
    }
});
