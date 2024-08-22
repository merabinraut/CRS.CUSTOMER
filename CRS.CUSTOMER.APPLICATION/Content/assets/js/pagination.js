function initializePagination(currentPage, totalPages, prefecturesArea, typeValue) {
    ClubPagination(currentPage, totalPages, prefecturesArea, typeValue);
    $('#prev-btn').on('click', function () {
        if (currentPage > 1) {
            changePage(currentPage - 1, totalPages, prefecturesArea, typeValue);
        }
    });
    $('#next-btn').on('click', function () {
        if (currentPage < totalPages) {
            changePage(currentPage + 1, totalPages, prefecturesArea, typeValue);
        }
    });
}
function ClubPagination(currentPage, totalPages, prefecturesArea, typeValue) {
    $('#page-numbers').empty();
    $('#prev-btn').prop('disabled', currentPage === 1);
    let startPage = Math.max(1, currentPage - 2);
    let endPage = Math.min(totalPages, currentPage + 2);

    if (currentPage - 2 < 1) {
        endPage = Math.min(5, totalPages);
    }
    if (currentPage + 2 > totalPages) {
        startPage = Math.max(1, totalPages - 4);
    }
    if (startPage > 1) {
        $('#page-numbers').append(createPageElement(1, prefecturesArea, typeValue));
        if (startPage > 2) {
            $('#page-numbers').append(createEllipsis());
        }
    }
    for (let i = startPage; i <= endPage; i++) {
        const pageElement = createPageElement(i, prefecturesArea, typeValue);
        if (i === currentPage) {
            pageElement.addClass('active');
        }
        $('#page-numbers').append(pageElement);
    }
    if (endPage < totalPages) {
        if (endPage < totalPages - 1) {
            $('#page-numbers').append(createEllipsis());
        }
        $('#page-numbers').append(createPageElement(totalPages, prefecturesArea, typeValue));
    }
    $('#next-btn').prop('disabled', currentPage === totalPages);
}
function createPageElement(page, prefecturesArea, typeValue) {
    return $('<div>', {
        text: page,
        click: function () {
            changePage(page, totalPages, prefecturesArea, typeValue);
        }
    });
}
function createEllipsis() {
    return $('<div>', { text: '...' });
}
function changePage(page, totalPages, prefecturesArea, typeValue) {
    ClubPagination(page, totalPages, prefecturesArea, typeValue);
    submitForm(page, prefecturesArea, typeValue);
}

function submitForm(page, prefecturesArea, typeValue) {
    const page_size = 25;
    const startIndex = (page - 1) * page_size;
    $('#startindex-id').val(startIndex);

    const form = document.getElementById("pagination-form-id");
    if (typeValue.trim().toUpperCase() === "CLUB") {
        form.action = `/search${prefecturesArea}/?target=${typeValue}`;
    } else {
        form.action = `/search${prefecturesArea}/?TopSearch=${typeValue}`;
    }
    form.submit();
}
