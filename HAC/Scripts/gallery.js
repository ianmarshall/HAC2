
var viewModel = {
    searchResult: ko.observableArray([]),

    thumbnails: ko.observableArray(),
    
    mainimageurl: ko.observable(''),
    imagepubliclink: ko.observable(''),
    comments: ko.observableArray(),
    latestcomments: ko.observableArray(),
    exif: ko.observableArray(),
    hasExifInfo: ko.observable(false),
    name: ko.observableArray(),
    friendlyname: ko.observable(''),
    imagedescr: ko.observable(''),
    removeComment: function (item) {
        this.comments.remove(item);
    }
   
};

viewModel.lastItem = ko.dependentObservable(function () {
    return this.thumbnails()[this.thumbnails().length - 1];
}, viewModel);


viewModel.applyThumbnailScroller = function (elements, data) {
    
    if (data == viewModel.lastItem()) {
        $("#tS2").thumbnailScroller({
             scrollerType: "clickButtons",
             scrollerOrientation: "horizontal",
             scrollSpeed: 800,
             scrollEasing: "easeOutCirc",
             scrollEasingAmount: 00,
             acceleration: 1,
             noScrollCenterSpace: 10,
             autoScrolling: 0,
             autoScrollingSpeed: 80000,
             autoScrollingEasing: "easeInOutQuad",
             autoScrollingDelay: 500
        });


    }
};



//ko.applyBindings(new viewModel());


/*prefetching*/
var cache_img = []
var cache_queue = [];

function PreFetchImagesInQueue() {
    if (cache_queue) {
        var img = cache_queue.shift();
        if (img) {
            PreFetchImg(img.url, img.rel, function () {
                //log ('loaded');
                PreFetchImagesInQueue();
            });
        }
    }
}

function PreFetchImg(img, rel, callback) {
    $.ajax({
        url: img, success: function (data) {
            if (rel) {
                var img = $("a[rel='" + rel + "'] img");
                //log (img);
                $(img).addClass("loaded");
                callback();
            }
        }
    });
}
/*end prefetching*/

/*logging*/
function log(log) {
    if (window.console && console.log)
        console.log(log);
}
/*end logging*/

function OpenFolder(folderVPath, callback) {
    var linkFolder = $("a[rel='" + folderVPath + "']");
    $(linkFolder).show();
    $(linkFolder).parents().show();
    //ApplyStyleSelectedFolder(folderVPath);
  
    loadThumbnailsByFolder(folderVPath, function (error, thumbnails) {
        if (!error) {
            DisplayThumbnails(thumbnails);
            if (callback)
                callback();
        }
    });
    
}

function IsFolderSelected(folder) {
    return $(folder).hasClass("selected");
}

function ApplyStyleSelectedFolder(folder) {
    $('#folderList a').removeClass("selected");
    $(folder).addClass("selected");
}

function ApplyStyleSelectedThumbnail(image) {
    $('ul#thumbnails li a img').removeClass("selected");
    $(image).addClass("selected");
}

function OpenNewComment() {
    $('#modalNewComment').modal('show');
    $('#Body').focus();
    return false;
}

function NextImage() {
    var linkNext = $('ul#thumbnails li a img.selected').closest('li').next().find(">:first-child").trigger('click');
    $(linkNext).trigger('click');
}

function PrevImage() {
    var linkPrev = $('ul#thumbnails li a img.selected').closest('li').prev().find(">:first-child");
    $(linkPrev).trigger('click');
}


function animateNew(element) {
    //$(element).hide().fadeIn();
    //$(element).animate({opacity: 0.25, height: 'toggle'}, 1000, function() {});
}

function existsElement(obj) {
    return ($(obj).length > 0);
}

function IsModalOpened() {
    return ($('#modalEditImageInfo').is(":visible")
        || $('#modalNewComment').is(":visible")
        || $('#modalExifData').is(":visible"));
}

function ShowMainArea(id) {
    if (!$('#' + id).is(':visible'))
        $('#' + id).fadeIn('fast');

    if (id != 'searchResults') {
        if ($('#searchResults').is(':visible'))
            $('#searchResults').fadeOut();
    }

    if (id != 'mainImageArea') {
        if ($('#mainImageArea').is(':visible'))
            $('#mainImageArea').fadeOut();
    }

    if (id != 'htmlContent') {
        if ($('#htmlContent').is(':visible'))
            $('#htmlContent').fadeOut();
    }

    if (id != 'latestCommentsArea') {
        if ($('#latestCommentsArea').is(':visible'))
            $('#latestCommentsArea').fadeOut();
    }
}

function LoadImage(url) {
    //$('#mainImageContainer img').fadeOut('fast'); //transition effect
  
    $.ajax({
        url: url, success: function (data) {

            
            viewModel.mainimageurl(data.ImageVPath);
            //viewModel.imagepubliclink(data.ImageMainPage);

         //   viewModel.comments(data.Comments);
          //  viewModel.exif(data.EXIF);

            //viewModel.hasExifInfo(data.EXIF.HasInfo);
            viewModel.name(data.Name);
            viewModel.friendlyname(data.ImageFriendlyName);
            viewModel.imagedescr(data.ImageDescr);

            ShowMainArea('mainImageArea');

            
            //if (data.NextThumbnail == "") {
            //    $('#arrowright').fadeOut('fast');
            //}
            //else {
            //    PreFetchImg(data.NextImage);
            //    $('#arrowright').fadeIn('fast');
            //}

            //if (data.PrevThumbnail == "") {
            //    $('#arrowleft').fadeOut('fast');
            //}
            //else {
            //    //fetch prev image
            //    PreFetchImg(data.PrevImage);
            //    $('#arrowleft').fadeIn('fast');
            //}
         

          //  $('#ImageVPath').val(data.ImageVPath);
          //  $('#ImageVPathEditImageInfo').val(data.ImageVPath);

            //$('#mainImageContainer img').fadeIn('fast');
        }
    });
}

/*Load thumbnails */
function loadThumbnailsByFolder(folderVPath, callback) {
    $.ajax({
        url: folderVPath, success: function (data) { //fetch thumbnails from folder          
            callback(null, data.thumbnails);
        }, error: function (request, error) {
           
            if (request.status == 403) {
            
                callback('Auth error', null);
            }
            else {
                callback('Error procesing request', null);
            }
        }
    });
}

/*display thumbnails in ui (knockout binding)*/
function DisplayThumbnails(thumbnails) {
    viewModel.thumbnails(thumbnails);
    var val_slider = $("#slider").slider("value");

    $('ul#thumbnails li a img').css("width", val_slider + "px");
    $('ul#thumbnails li a img').css("height", val_slider + "px");
}


/*Load thumbnails */
function loadThumbnails(url) {
    cache_queue = [];
   
    $.ajax({
        url: url,
        success: function (data) { //fetch thumbnails from folder          

         
            DisplayHomeThumbnails(data.thumbnails);
            

            for (var i = 0; i < data.thumbnails.length; i++) {
                var img = { url: data.thumbnails[i].UrlFullImage, rel: data.thumbnails[i].VPath };
                cache_queue.push(img);
            }

            setTimeout(PreFetchImagesInQueue, 2000); //2 sec delay

        }, error: function (xhr, ajaxOptions, thrownError) {
            alert(xhr.status);
            alert(thrownError);
        }
    });

    return;
}

/*display thumbnails in ui (knockout binding)*/

function DisplayHomeThumbnails(thumbnails) {
    viewModel.thumbnails(thumbnails);

    viewModel.thumbnails.applyThumbnailScroller();
}


function loadThumbnailsByFolderLink(link, autoclick_first_thumbnail, thumbnail_to_open) {
    var url = $(link).attr('href');

 

    cache_queue = [];


    $.ajax({
        url: url, success: function (data) { //fetch thumbnails from folder          
            if (IsFolderSelected($(link))) { //back with data. we just show it if the user didn't move to other folder to avoid flickering
                DisplayThumbnails(data.thumbnails);

                for (var i = 0; i < data.thumbnails.length; i++) {
                    var img = { url: data.thumbnails[i].UrlFullImage, rel: data.thumbnails[i].VPath };
                    cache_queue.push(img);
                }

                setTimeout(PreFetchImagesInQueue, 2000); //2 sec delay

                $('#latestCommentsArea').hide();
                if (data.htmlcontent) { //we have html to show
                    ShowMainArea('htmlContent');
                    $('#htmlContent').html(data.htmlcontent);
                }
                else if (data.thumbnails.length > 0) { //we don't, so load first thumbnails
                    ShowMainArea('mainImageArea');
                    if (autoclick_first_thumbnail)
                        $('#thumbnails a:first').trigger('click');
                    else if (thumbnail_to_open && thumbnail_to_open.length > 0) {
                        var linkThumbnail = $("a[href='" + thumbnail_to_open + "']");
                    
                        $(linkThumbnail).trigger('click');
                    }
                }
                else { //no htmlcontent nor thumbs
                    $('#mainImageArea').fadeOut();
                    $('#htmlContent').fadeOut();
                }

            }
        }, error: function (request, error) {
            if (request.status == 403) {
                ShowMainArea('htmlContent');
                $('#htmlContent').html('<div class="alert-message error">Access to this folder is restricted</div>');
                viewModel.thumbnails([]);
            }
            else {
                //other error
                alert('Error processing request');
            }
        }
    });
}


function folderListClick(link, show, autoclick_first_thumbnail, thumbnail_to_open) {

    var url = $(link).attr('href');

   
    ApplyStyleSelectedFolder(link);

    if (show) {
        $(link).next().show("slow");
        $(link).closest('ul').show("slow");

        if ($(link).parent().hasClass('subfolder')) { //no subfolders
            $('li.subfolder').removeClass("open");
        }
        $(link).closest('li').addClass("open");
    }
    else {
        $(link).next().hide("slow");
        $(link).closest('li').removeClass("open");
    }

    loadThumbnailsByFolderLink(link, autoclick_first_thumbnail, thumbnail_to_open);

}


$(document).ready(function () {

    ko.applyBindings(viewModel);

    function search(term) {
        $('#searchloading').fadeIn();
        $.ajax({
            url: rootPath + 'Search/Search?q=' + encodeURIComponent(term), success: function (data) { //fetch thumbnails from folder          
                viewModel.searchResult(data.Folders);
                ShowMainArea('searchResults');
                $('#searchloading').fadeOut();
            },
            error: function (request, error) {
                $('#searchloading').fadeOut();
                /*
                if (request.status==403){
                    $('#mainImageArea').fadeOut();
                    $('#htmlContent').fadeIn();
                    $('#htmlContent').html('<div class="alert-message error">Access to this folder is restricted</div>');
                }
                else{
                    //other error
                    alert('Error processing request');
                }
                */
                alert('error');
            }
        });
    }

    //search box    
    var lastsearch = "";
    $('#searchBox').keydown(function (e) {
        var content = $('#searchBox').val();

        if (content.length == 0) {
            viewModel.searchResult([]);
        }
        else if (e.keyCode == '13') {
            lastsearch = content;
            search(content);
        }
        else if (content.length > 2) {
            if (Math.abs(lastsearch.length - content.length) > 2) {
                lastsearch = content;
                search(content);
            }
        }
    });


    /*share*/
    $('a.share_btn').live('click', function () {
        $('div.share_content').toggle('fade');
        $('input.share_textbox').select();
        $(this).hide();
        return false;
    });

    $('input.share_textbox').live('click', function () {
        $(this).select();
        return false;
    })

    $('a.close_share').live('click', function () {
        $('div.share_content').hide();
        $('a.share_btn').show();
        return false;
    })


    $('#ImageFriendlyName').select();

    /*show and hide delete comment*/
    $('div.comment').live('mouseover', function () {
        if (!$('.messageAction', this).is(':visible')) {
            $('.btoShowMessageDialog', this).show();
        }
    })

    $('div.comment').live('mouseout', function () {
        $('.btoShowMessageDialog', this).hide();
    })

    /*click on delete button x*/
    $('input.btoShowMessageDialog').live('click', function () {
        $(this).hide();
        var comment = $(this).parent();
        $('.messageAction', comment).fadeIn(300);
        $('.messageAction form', comment).fadeIn(300);
    })

    /*cancel operation message or comment*/
    $('.messageAction input.cancel').live('click', function (e) {
        var comment = $(this).closest('div.comment');
        $('.messageAction', comment).fadeOut(500);
        $('.messageAction form', comment).fadeOut(500);
        return false;
    })

    $('.messageAction input.delete').live('click', function (e) {
        var id = $(this).attr("id");
        var type = $(this).attr("type");

        $.ajax({
            type: "POST",
            url: rootPath + "Comment/Delete",
            data: "id=" + id,
            success: function (data) {
                for (var i = 0; i < viewModel.comments().length; i++) {
                    if (viewModel.comments()[i].ID == id) {
                        viewModel.removeComment(viewModel.comments()[i]);
                    }
                }

                for (var i = 0; i < viewModel.latestcomments().length; i++) {
                    if (viewModel.latestcomments()[i].ID == id) {
                        viewModel.removeLatestComment(viewModel.latestcomments()[i]);
                    }
                }
            },
            error: function (data, status) {
                alert('Error deleting comment. Error:' + JSON.parse(data.responseText).error);
                log(data);
            },
        });

        return false;
    });


    //arrows left and right
    $('a#arrowleft').live('click', function () {
        PrevImage();
        return false;
    });

    $('a#arrowright').live('click', function () {
        NextImage();
        return false;
    });

    //open exif data
    $('a#openExifData').live('click', function () {
        $('#modalExifData').modal('show');
        return false;
    });

    //edit image info
    $('a#btoEditImageInfo').live('click', function () {
        $('#modalEditImageInfo').modal('show');
        $('#ImageFriendlyName').focus();
        $('#ImageFriendlyName').select();
        return false;
    });

    //latestComments
    $('a#latestComments').live('click', function () {
        $.ajax({
            url: $(this).attr("href"), success: function (data) {
                viewModel.latestcomments(data.Comments);
                ShowMainArea('latestCommentsArea');
            }
        });
        return false;
    });

    //new comment
    $('a#btonewcomment').live('click', function () {
        return OpenNewComment();
    });

    $('.closemodal').live('click', function () {
        $('#modalEditImageInfo').modal('hide');
        $('#modalNewComment').modal('hide');
        $('#modalExifData').modal('hide');
        return false;
    });


    $('form#editImageInfo').live('submit', function () {
        $.post($(this).attr('action'), $(this).serialize(), function (data, status) {
            //viewModel.comments.splice (0,1,data.Comments[0]);
            viewModel.imagedescr(data.ImageDescr);
            viewModel.friendlyname(data.ImageFriendlyName);

            $('#modalEditImageInfo').modal('hide');
            return false;

        }).error(function (error) {
            alert(error.error);
            return false;
        });

        return false;
    });

    $('form#newcomment').live('submit', function () {
        $('.submit').attr("disabled", true);
        $('#modalNewComment').modal('hide');

        $.post($(this).attr('action'), $(this).serialize(), function (data, status) {
            viewModel.comments(data.Comments);
            $('#Body').val('');
            $('.submit').attr("disabled", false);
            return false;
        }).error(function (error) {
            $('#modalNewComment').modal('show');
            alert('Error: The comment has not been saved. Please check your input data. HTML is not allowed.');
            $('.submit').attr("disabled", false);
            return false;
        });

        return false;
    });


    //click on link to image
    $('a.linkSearchResult').live('click', function (e) {
        var foldervPath = $(this).attr('folder');
        var picturevPath = $(this).attr('picture');
        OpenFolder(foldervPath, function OnLoaded() {
            var alinkThumb = $("a[rel='" + picturevPath + "']");
            var image = $(alinkThumb).find(">:first-child");
            ApplyStyleSelectedThumbnail(image); //mark as selected
            LoadImage(rootPath + 'Image?ImageVPath=' + encodeURIComponent(picturevPath));
        });
        return false;
    })



    //click on an image thumbnails: display main image
    $('ul#thumbnails li a').live('click', function (e) {
        var image = $(this).find(">:first-child"); //.css("width", "100px");            
        ApplyStyleSelectedThumbnail(image); //mark as selected
        LoadImage(this.href);
        return false;
    })


    //Treeview
    $('#folderList a').live('click', function (e) {
        e.preventDefault();
        var show = true;
        if ($(this).next('ul').is(':visible')) {
            show = false;
        }
        folderListClick($(this), show, true);
    })


    //config slider
    $("#slider").slider({ value: 50 });
    $("#slider").slider({
        slide: function (event, ui) {
            $('ul#thumbnails li a img').css("width", ui.value + "px");
            $('ul#thumbnails li a img').css("height", ui.value + "px");
        }
    });



    $(document).keydown(function (e) {

        if (e.altKey && e.keyCode == 67) { // 'c'. new comment
            return OpenNewComment();
        }

        if (e.keyCode == 27) { //esc
            $('#modalNewComment').modal('hide');
            $('#ExifData').modal('hide');
            return false;
        }

        if (e.keyCode == 37) { //prev
            if (!IsModalOpened()) {
                PrevImage();
                return false;
            }
        }

        if (e.keyCode == 38) { //up
            if (!IsModalOpened()) {
                var currentfolder = $('#folderList ul a.selected').closest('li');
                var folderPrev = $(currentfolder).prev('li');
                if (existsElement(folderPrev)) {
                    folderListClick($(folderPrev).find(">:first-child"), true, true);
                }
                else {
                    //parent?
                    var parentFolder = $(currentfolder).closest('ul').closest('li');
                    var link = $(parentFolder).find(">:first-child");
                    if (existsElement(link)) {

                        folderListClick(link, true, true);
                    }

                }
                return false;
            }
        }


        if (e.keyCode == 39) { //next
            if (!IsModalOpened()) {
                NextImage();
                return false;
            }
        }


        if (e.keyCode == 40) { //down
            if (!IsModalOpened()) {
                function GetNextDown(folder) {
                    if (existsElement(folder)) {
                        folderListClick($(folder).find('>:first-child'), false, true);
                        if (existsElement(folder.next('li'))) {
                            var link = $(folder.next('li')).find('>:first-child');
                            folderListClick($(link), true, true);
                            return false;
                        }
                        else {
                            folder = $(folder).closest('ul').closest('li');
                            return GetNextDown(folder);
                        }
                    }
                    else {
                        return false;
                    }
                }

                var currentfolder = $('#folderList ul a.selected').closest('li');

                var subfolder = $(currentfolder).find('li')[0]; //has any children folder?
                if (existsElement(subfolder)) {
                    folderListClick($(subfolder).find('>:first-child'), true, true);
                }
                else {
                    //no children folder. any silbing?
                    return GetNextDown(currentfolder);
                }
                return false;
            }
        }
    });
}
);