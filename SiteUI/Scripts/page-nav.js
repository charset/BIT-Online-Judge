
var onPageSwitch = null;

$('#previousPageButton').click(function () {
    var currentPage = Number($('#previousPageButton').data('current'));
    onPageSwitch(currentPage - 1);
});

$('#nextPageButton').click(function () {
    var currentPage = Number($('#previousPageButton').data('current'));
    onPageSwitch(currentPage + 1);
});

$('#pageButton').click(function () {
    var toPage = Number($(this).data('page'));
    onPageSwitch(toPage);
});

function createPaginationHtml(pages, currentPage, onSwitch) {
    var pagination = '<nav id="pageNav" data-current="' + currentPage.toString() + '" aria-label="Page navigation"><ul class="pagination">';

    if (currentPage == 1)
        pagination += '<li class="disabled"><span aria-hidden="true">&laquo;</span></li>';
    else {
        pagination += '<li>';
        pagination += '<a href="#" id="previousPageButton" aria-label="Previous"><span aria-hidden="true">&laquo;</span></a>';
        pagination += '</li>';
    }

    if (pages <= 5) {
        for (var current = 1; current <= pages; ++current) {
            if (currentPage == current)
                pagination += '<li class="active"><span>' + current.toString() + '</span></li>';
            else {
                pagination += '<li>';
                pagination += '<a href="#" id="pageButton" data-page="' + current.toString() + '">';
                pagination += current.toString();
                pagination += '</a>';
                pagination += '</li>';
            }
        }
    } else {
        var displayPages = new Array(1, currentPage - 1, currentPage, currentPage + 1, pages);
        for (var index in displayPages) {
            if (index > 0 && displayPages[index] <= displayPages[index - 1])
                continue;
            else if (index > 0 && displayPages[index] > displayPages[index - 1] + 1)
                pagination += '<li class="disabled"><span>...</span></li>';

            if (displayPages[index] == currentPage)
                pagination += '<li class="active"><span>' + currentPage.toString() + '</span></li>';
            else {
                pagination += '<li>';
                pagination += '<a href="#" id="pageButton" data-page="' + displayPages[index].toString() + '">';
                pagination += '<span>' + displayPages[index].toString() + '</span>';
                pagination += '</a>';
                pagination += '</li>';
            }
        }
    }

    if (currentPage == pages)
        pagination += '<li class="disabled"><span aria-hidden="true">&raquo;</span></li>';
    else {
        pagination += '<li>';
        pagination += '<a href="#" id="nextPageButton" aria-label="Next"><span aria-hidden="false">&raquo;</span></a>';
        pagination += '</li>';
    }

    pagination += '</ul></nav>';

    onPageSwitch = onSwitch;
    return pagination;
}
