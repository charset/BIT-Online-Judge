
var ajaxPagesUrl = "/Query/ProblemListPages";
var ajaxProblemListUrl = "/Query/ProblemList";

function getProblemUrl(problemId) {
    return "/Archieve/ShowProblem?id=" + problemId;
}

function getProblemAcceptRatioText(totalAccepted, totalSubmissions) {
    var textTemplate = "{0} ( {1} / {2} )";
    if (totalSubmissions == 0) {
        textTemplate = textTemplate.replace("{0}", "N/A");
    } else {
        var ratio = totalAccepted * 100 / totalSubmissions;
        textTemplate = textTemplate.replace("{0}", ratio.toFixed(2) + "%");
    }

    return textTemplate
        .replace("{1}", totalAccepted.toString())
        .replace("{2}", totalSubmissions.toString());
}

function updateProblemList(title, author, source, origin, page, onfinished) {
    // Update pagination.
    var ajaxUrl = (ajaxPagesUrl + "?title={0}&author={1}&source={2}&origin={3}")
        .replace("{0}", title == null ? "" : title)
        .replace("{1}", author == null ? "" : author)
        .replace("{2}", source == null ? "" : source)
        .replace("{3}", origin == null ? "" : origin)
    $.get(ajaxPagesUrl, function (data, status) {
        var response = JSON.parse(data);
        if (validateQueryModel(response)) {
            var pages = response.data;
            $('#pageNav').children().remove();
            $('#pageNav').append(createPaginationHtml(pages, page, function (pg) {
                // Query problems on the specified page.
                updateProblemList(title, author, source, origin, pg, null);
            }));
        }
    });

    // Update list.
    ajaxUrl = (ajaxProblemListUrl + "?title={0}&author={1}&source={2}&origin={3}&page={4}")
        .replace("{0}", title == null ? "" : title)
        .replace("{1}", author == null ? "" : author)
        .replace("{2}", source == null ? "" : source)
        .replace("{3}", origin == null ? "" : origin)
        .replace("{4}", page == null ? "" : page);
    $.get(ajaxUrl, function (data, status) {
        var response = JSON.parse(data);
        if (validateQueryModel(response)) {
            // Update problem list.
            var itemHtmlTemplate = '<tr><td>{0}</td><td><a href="{1}" target="_self"><strong>{2}</strong></a></td><td>{3}</td></tr>';

            // Clear all items.
            $('#problemList > tbody').children().remove();

            for (var index in response.data) {
                var item = response.data[index];
                var itemHtml = itemHtmlTemplate
                    .replace("{0}", item.problemId)
                    .replace("{1}", getProblemUrl(item.problemId))
                    .replace("{2}", item.title)
                    .replace("{3}", getProblemAcceptRatioText(item.acsub, item.totalsub));

                $('#problemList > tbody').append(itemHtml);
            }
        }

        if (onfinished != null)
            onfinished();
    });
}

$('#queryForm').submit(function () {
    event.preventDefault();

    var title = $('#title').val();
    var author = $('#author').val();
    var source = $('#source').val();
    var origin = $('#origin').val();
    var page = $('#query_page').val();

    $('#title').addClass('disabled');
    $('#author').addClass('disabled');
    $('#source').addClass('disabled');
    $('#origin').addClass('disabled');
    $('#queryButton').addClass('disabled');
    
    updateProblemList(title, author, source, origin, page, function () {
        $('#title').removeClass('disabled');
        $('#author').removeClass('disabled');
        $('#source').removeClass('disabled');
        $('#origin').removeClass('disabled');
        $('#queryButton').removeClass('disabled');
    });

    return false;
});

updateProblemList($('#query_title').val(), $('#query_author').val(), $('#query_source').val(),
    $('#query_origin').val(), 1, null);
