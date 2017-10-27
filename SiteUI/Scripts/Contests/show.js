
function getContestStatusString(status) {
    if (status === 0)
        return 'Pending';
    else if (status === 1)
        return 'Running';
    else
        return 'Ended';
}

function getContestStatusClassName(status) {
    if (status === 0)
        return 'text-contest-pending';
    else if (status === 1)
        return 'text-contest-running';
    else
        return 'text-contest-ended';
}

function getContestParticipateModeString(partMode) {
    if (partMode === 1)
        return 'Individual ONLY';
    else if (partMode === 2)
        return 'Teamwork ONLY';
    else
        return 'Individual AND Teamwork';
}

function getContestAuthorizationModeString(authMode) {
    if (authMode === 0)
        return 'Private';
    else if (authMode === 1)
        return 'Protected';
    else
        return 'Public';
}

function getContestAuthorizationModeClassName(authMode) {
    if (authMode === 0)
        return 'text-contest-private';
    else if (authMode === 1)
        return 'text-contest-protected';
    else
        return 'text-contest-public';
}

function getProblemRowHtml(index, problemModel) {
    var html = '<tr>';

    html += '<td>' + index + '</td>';
    html += '<td></td>';
    html += '<td><a href="/Archieve/ShowProblem?id=' + problemModel.problemId + '" target="_blank"><strong>'
        + problemModel.title + '</strong></a></td>';

    html += '<td>';
    html += '<p>' + problemModel.totalsub + ' submissions, ' + problemModel.acsub + ' accepted. </p>';
    html += '<p> Accept ratio: ';
    if (problemModel.totalsub === 0)
        html += 'N/A';
    else {
        var ratio = problemModel.acsub / problemModel.totalsub * 100;
        html += ratio.toFixed(2) + ' %';
    }
    html += '</p>';
    html += '</td>';

    html += '</tr>';
    return html;
}

function getTimespanString(timespan) {
    var days = parseInt(timespan / 1000 / 3600 / 24);
    var hours = parseInt(timespan / 1000 / 3600 % 24);
    var minutes = parseInt(timespan / 1000 / 60 % 60);
    var seconds = parseInt(timespan / 1000 % 60);

    return days + ' d ' + hours + ' h ' + minutes + ' m ' + seconds + ' s';
}

function updateContestProgress(startTime, endTime, status) {
    var nowTime = new Date();

    var nowstatus = null;
    if (nowTime < startTime) {
        // Contest is pending.
        nowstatus = 0;

        var timespan = startTime - nowTime;

        $('#contestCountdown').html(getTimespanString(timespan));
        $('#contestProgress > .progress-bar').width('0');
        $('#contestProgress > .progress-bar').attr('aria-valuenow', '0');
    } else if (nowTime <= endTime) {
        // Contest is running.
        nowstatus = 1;

        var timespan = endTime - nowTime;
        var percentage = (nowTime.getTime() - startTime.getTime()) / (endTime.getTime() - startTime.getTime()) * 100;

        $('#contestCountdown').html(getTimespanString(timespan));
        $('#contestProgress > .progress-bar').width(percentage + '%');
        $('#contestProgress > .progress-bar').attr('aria-valuenow', percentage);
    } else {
        // Contest is ended.
        nowstatus = 2;

        $('#contestCountdown').html('00:00:00');
        $('#contestProgress > .progress-bar').width('100%');
        $('#contestProgress > .progress-bar').attr('aria-valuenow', 100);
        $('#contestProgress').removeClass('active');
    }

    if (nowstatus != status) {
        // Contest status is updated.
        updateContestDisplay();
    }

    if (nowstatus != 2) {
        // Contest is not ended.
        window.setTimeout(updateContestProgress, 1000, startTime, endTime, nowstatus);
    }
}

function updateContestDisplay() {
    var detailQueryUrl = "/Query/ContestDetail?id=" + $('#contestId').val();
    var registerIdQueryUrl = "/Query/ContestRegisterIdentity?id=" + $('#contestId').val();
    var accessQueryUrl = "/Query/ContestAccess?id=" + $('#contestId').val();

    // Fetch contest display information from server.
    $.get(detailQueryUrl, function (data, status) {
        (function (model) {
            $('#contestTitle').html(model.title);
            if (model.creator !== null && model.creator !== '') {
                $('#contestCreator').html('by ' + model.creator);
                $('#contestCreator').removeAttr('hidden');
            }

            $('#contestStartTime').html(model.startTime);
            $('#contestEndTime').html(model.endTime);

            $('#contestStatus').html(getContestStatusString(model.status));
            $('#contestAuthMode').html(getContestAuthorizationModeString(model.authMode));
            $('#contestPartMode').html(getContestParticipateModeString(model.partMode));
            $('#contestStatus').addClass(getContestStatusClassName(model.status));
            $('#contestAuthMode').addClass(getContestAuthorizationModeClassName(model.authMode));

            $('#contestProblemList > tbody > *').remove();
            for (var i = 0; i < model.problems.length; ++i)
                $('#contestProblemList > tbody').append(getProblemRowHtml(i + 1, model.problems[i]));

            if (model.announcement === null || model.announcement === undefined || model.announcement === '')
                $('#contestAnnouncement').html('No published announcement yet.');
            else
                $('#contestAnnouncement').html(model.announcement);

            // Update time trial and contest progress.
            var startTime = new Date();
            startTime.setTime(Date.parse(model.startTime));

            var endTime = new Date();
            endTime.setTime(Date.parse(model.endTime));

            updateContestProgress(startTime, endTime, model.status);
        })(JSON.parse(data).data);
    });

    // Fetch register identity from server.
    $.get(registerIdQueryUrl, function (data, status) {
        (function (model) {
            $('#contestRegId').html(model);
        })(JSON.parse(data).data);
    });

    // Fetch user data access from server.
    $.get(accessQueryUrl, function (data, status) {
        (function (model) {
            if ((model & 0x00000002) !== 0)
                $('#contestWriteRegion').removeAttr('hidden');
        })(JSON.parse(data).data);
    });
}

$('#publishAnnouncementButton').click(function () {
    $('#announcementForm').submit();
});

$('#announcementForm').submit(function () {
    event.preventDefault();

    $('#publishAnnouncementButton').addClass('disabled');
    $('#announcementContent').attr('readonly', 'readonly');

    var actionUrl = '/Contest/SetAnnouncement?id=' + $('#contestId').val();
    $.post(actionUrl, $('#announcementForm').serialize(), function (data, status) {
        updateContestDisplay();

        $('#publishAnnouncementButton').removeClass('disabled');
        $('#announcementContent').removeAttr('readonly');

        $('#announcementDialog').modal('hide');
    });

    return false;
});

$('#addArchieveProblemButton').click(function () {
    $('#addArchieveProblemForm').submit();
});

$('#addArchieveProblemForm').submit(function () {
    event.preventDefault();

    $('#ProblemId').attr('readonly', 'readonly');
    $('#addArchieveProblemForm').attr('disabled', 'disabled');

    var contestId = $('#contestId').val();
    var problemId = $('#ProblemId').val();
    if (problemId === null || problemId.length === 0) {
        // 用户未输入任何内容。
        $('#ProblemId').parent("div")[0].classList.add("has-error");
        $('#ProblemId').removeAttr('readonly');
        $('#addArchieveProblemForm').removeAttr('disabled');

        return false;
    }

    var form = $('#addArchieveProblemForm');
    var requestUrl = form.attr('action') + '?contestId=' + contestId;
    var data = form.serialize();
    var currentUrl = window.location;

    $.post(requestUrl, data, function (data, status) {
        window.location = currentUrl;
    });

    return false;
});

$('#deleteContestButton').click(function () {
    $('#deleteContestButton').attr('disabled', 'disabled');

    var actionUrl = '/Contest/Delete?id=' + $('#contestId').val();
    $.post(actionUrl, function (data, status) {
        window.location = '/Contest';
    });
});

updateContestDisplay();
