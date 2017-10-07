
var ajaxProblemDetailUrl = '/Query/ProblemDetail?id=' + $('#problemIdFeed').val();
$.get(ajaxProblemDetailUrl, function (data, status) {
    var response = JSON.parse(data);
    if (validateQueryModel(response)) {
        var data = response.data;
        document.title = 'BITOJ - ' + data.title;
        $('.jumbotron > h1').html(data.title + '<small>Loading...</small>');
        $('.jumbotron > h1 > small').html('by ' + data.author);
        $('#timeLimit').html(data.timeLimit);
        $('#memoryLimit').html(data.memoryLimit);
        $('#specialJudge').html(data.specialJudge ? 'Yes' : 'No');
        $('#problemId').html(data.problemId);
        $('#title').html(data.title);
        $('#description').html(data.description);
        $('#inputDescription').html(data.inputDescription);
        $('#outputDescription').html(data.outputDescription);
        $('#inputExample').html(data.inputExample);
        $('#outputExample').html(data.outputExample);
        $('#hint').html(data.hint);
        $('#source').html(data.source);
        $('#author').html(data.author);

        if (data.hint == null || data.hint == undefined || data.hint == '')
            $('#hintBox').hide();
        if (data.source == null || data.source == undefined || data.source == '')
            $('#sourceBox').hide();
        if (data.author == null || data.author == undefined || data.author == '') {
            $('#authorBox').hide();
            $('.jumbotron > h1 > small').hide();
        }
    }
});

$('#deleteProblemButton').click(function () {
    $('#deleteProblemButton').attr('disabled', 'disabled');
    var url = '/Archieve/Delete?id=' + $('#problemIdFeed').val();
    $.post(url, function (data, status) {
        location.assign('/Archieve');
    });
});
