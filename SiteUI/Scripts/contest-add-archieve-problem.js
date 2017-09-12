
$('#addArchieveProblemButton').click(function () {
    $('#addArchieveProblemForm').submit();
});

$('#addArchieveProblemForm').submit(function () {
    event.preventDefault();

    $('#ProblemId').attr('readonly', 'readonly');
    $('#addArchieveProblemForm').attr('disabled', 'disabled');

    var contestId = $('#contestId').val();
    var problemId = $('#ProblemId').val();
    if (problemId == null || problemId.length == 0) {
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
