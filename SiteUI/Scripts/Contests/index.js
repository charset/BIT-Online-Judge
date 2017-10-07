
var ajaxPageUrl = '/Query/ContestListPages';
var ajaxContestListUrl = '/Query/ContestList';

function getContestStatusClassName(status) {
    if (status === 0)
        return 'text-contest-pending';
    else if (status === 1)
        return 'text-contest-running';
    else
        return 'text-contest-ended';
}

function getContestStatusName(status) {
    if (status === 0)
        return 'Pending';
    else if (status === 1)
        return 'Running';
    else if (status == 2)
        return 'Ended';
    else
        return '';
}

function getContestRowHtml(contest) {
    var contestHtml = '<tr>';

    contestHtml += '<td>';
    contestHtml += '<a href="/Contest/Show?id=' + contest.id + '"><strong>' + contest.title + '</strong><a/>';
    contestHtml += '</td>';

    contestHtml += '<td>';
    contestHtml += '<span class="' + getContestStatusClassName(contest.status) + '">' + getContestStatusName(contest.status) + '</span>';
    contestHtml += '</td>';

    contestHtml += '<td>' + contest.creator + '</td>';
    contestHtml += '<td>' + contest.startTime + '</td>';

    contestHtml += '</tr>';

    return contestHtml;
}

function updateContestList(title, creator, status, page, onfinished) {
    // Query pages.
    var ajaxUrl = (ajaxPageUrl + '?title={0}&creator={1}&status={2}')
        .replace('{0}', title === null ? '' : title)
        .replace('{1}', creator === null ? '' : creator)
        .replace('{2}', status === null ? '' : status);
    $.get(ajaxUrl, function (data, st) {
        // Update pagination url.
        var response = JSON.parse(data);
        if (validateQueryModel(response)) {
            $('#pageNav').children().remove();
            $('#pageNav').append(createPaginationHtml(response.data, page, function (pg) {
                updateContestList(title, author, status, pg);
            }));
        }
    });

    // Query contest list.
    ajaxUrl = (ajaxContestListUrl + '?title={0}&creator={1}&status={2}')
        .replace('{0}', title === null ? '' : title)
        .replace('{1}', creator === null ? '' : creator)
        .replace('{2}', status === null ? '' : status)
        .replace('{3}', page === null ? 1 : page);
    $.get(ajaxUrl, function (data, st) {
        var response = JSON.parse(data);
        if (validateQueryModel(response)) {
            $('#contestListBody').children().remove();
            for (var index in response.data) {
                var contest = response.data[index];
                $('#contestListBody').append(getContestRowHtml(contest));
            }
        }
    });

    if (onfinished !== null)
        onfinished();
}

$('#queryForm').submit(function () {
    event.preventDefault();

    var title = $('#title').val();
    var creator = $('#creator').val();
    var status = getContestStatusName(Number($('#status').val()));

    $('#title').addClass('disabled');
    $('#creator').addClass('disabled');
    $('#status').addClass('disabled');
    $('#queryButton').addClass('disabled');

    updateContestList(title, creator, status, 1, function () {
        $('#title').removeClass('disabled');
        $('#creator').removeClass('disabled');
        $('#status').removeClass('disabled');
        $('#queryButton').removeClass('disabled');
    });
});

updateContestList(null, null, null, 1, null);
