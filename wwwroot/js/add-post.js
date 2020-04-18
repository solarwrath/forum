function setAddPostEvent(addPostAction) {
    const addPostForm = $('#add-post-form');

    let messageInput = $('textarea[name="addPostMessage"]');
    const topicId = addPostForm.data('topic-id');

    addPostForm.submit((event) => {
        const requestData = {
            Message: messageInput.val(),
            TopicId: topicId,
        };

        $.ajax({
            method: "POST",
            url: addPostAction,
            data: JSON.stringify(requestData),
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: (response) => {
                //TODO Scroll to the end
                //Can update DOM but it would be hardcoded (not partial view) because of no model 
                //Or inconsistent with actual model
                if (response.success) {
                    messageInput.addClass('pristine');
                    messageInput.val('');

                    window.location.reload();
                } else {
                    handleAddPostError();
                }
            },
            error: handleAddPostError
        });

        event.preventDefault();
    });

    messageInput.focus(() => {
        messageInput.removeClass('pristine');
    });
}

function handleAddPostError() {
    const alertElement = $(`<div class="alert alert-danger alert-dismissible fade show" role="alert">
                                <strong>ERROR!</strong> Something went wrong; couldn't add your post!
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>`);
    alertElement.prependTo('.main-content');

    setTimeout(() => {
        $(alertElement).slideUp("slow", () => {
            $(alertElement).remove();
        });
    }, 5000);
}

function findLastPostId() {
    const lastElement = $('div[id^="post-"]').last();
    const lastElementId = lastElement.attr('id');

    return lastElementId.substring("post-".length);
}