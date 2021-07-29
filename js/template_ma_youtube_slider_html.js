class YoutubeVideo {
  constructor(id, desc, font, color, volume, cc) {
    this.video_id = id;
    this.desc = desc;
    this.desc_font = font;
    this.desc_font_color = color;
    this.volume = volume;
    this.cc = cc;
  }
}

function TemplateMaYoutubeSlider(debug) {
  TemplateMaBase.call(this);
  let _this = this;

  // empty video objects array
  let videos = [];

  this.templateName = "ma_youtube_html";

  this.playTemplate = function () {

    this.buildTemplateHeader();
    this.buildTemplateFooter();
  }

  this.initEditView = function () {
    this.m_playmode = "editor";
    this.playTemplate();

  }

  this.updateEditView = function () {
    this.initEditView();
  }

  // fills videos array
  this.setVideos = function () {
    // start count at 1 for s1 slide data
    let i = 1;
    // keep track of current video
    let currentVideo = {};
    // start at s1
    currentVideo.video_id = _this.contentField.s1_youtube_video_id;

    while(currentVideo.video_id != undefined && currentVideo.video_id != '' && i <= 30){
      // while there is a video id, add properties from data file dynamically
      eval("currentVideo.video_id = (_this.contentField.s" + i + "_youtube_video_id == undefined) ? '' : _this.contentField.s" + i + "_youtube_video_id;");
      if(currentVideo.video_id == undefined || currentVideo.video_id == ''){break;}
      eval("currentVideo.desc = (_this.contentField.s" + i + "_video_desc == undefined) ? '' : _this.contentField.s" + i + "_video_desc;");
      eval("currentVideo.desc_font_color = (_this.contentField.s" + i + "_video_desc_font_color == undefined || _this.contentField.s" + i + "_video_desc_font_color.length == 0) ? '000000' : _this.contentField.s" + i + "_video_desc_font_color;");
      eval("currentVideo.desc_font = (_this.contentField.s" + i + "_video_desc_font == undefined || _this.contentField.s" + i + "_video_desc_font.length == 0) ? 'Arial' : _this.contentField.s" + i + "_video_desc_font;");
      eval("currentVideo.volume = (_this.contentField.s" + i + "_volume == undefined || _this.contentField.s" + i + "_volume < 0) ? 5 : _this.contentField.s" + i + "_volume;");
      eval("currentVideo.cc = (_this.contentField.s" + i + "_cc == undefined) ? true : _this.contentField.s" + i + "_cc;");
      i++; // next index
      // set new obj to push;
      videos.push(new YoutubeVideo(currentVideo.video_id, currentVideo.desc, currentVideo.desc_font, currentVideo.desc_font_color, currentVideo.volume, currentVideo.cc)); // add to list of videos
    }

    // set video count CSS variable
    document.documentElement.style.setProperty('--maxVideos', i);

  }

  this.buildBody = function () {

    this.setVideos();

    for(let i = 0; i < videos.length; i++){
      let desc = videos[i].desc;
      let descFontColor = videos[i].desc_font_color;
      let descFont = videos[i].desc_font;

      $('ol').append('<li class="item"><div width="100%" controls id="video' + i + '"></div><div id="desc' + i + '"></div></li>');

      //use variables to set elements

      $('#desc' + i).css('color', "#" + descFontColor);
      $('#desc' + i).css('font-family', descFont);
      $('#desc' + i).css('font-size', "18px");
      $('#desc' + i).css('margin', "10px");
      $('#desc' + i).html(desc);
    }
    
    // slide color default white if not found
    let slideColor = formatColor(_this.contentField.slide_bg_color);
    if(isColor(slideColor)){document.documentElement.style.setProperty('--slideColor', slideColor);}

    var containerHeight = $('#template_container').outerHeight() + ($('#template_container').outerHeight() * .1);
    parent.updateiFrameHeight("template_" + _this.contentId, containerHeight);

  }

  this.onYouTubeIframeAPIReady = function () {
    this.buildBody();
    for(let i = 0; i < videos.length; i++){
      let youtubeVideoId = videos[i].video_id;
      if (youtubeVideoId.indexOf('?v=') != -1) {
        youtubeVideoId = youtubeVideoId.split('v=')[1];
      } else if (youtubeVideoId.indexOf('.be/') != -1) {
        youtubeVideoId = youtubeVideoId.split('.be/')[1];
      }
      _this.player = new YT.Player('video' + i, {
        // height: '66%',
        width: '100%',
        events: {
          'onReady': _this.onPlayerReady
        },
        videoId: youtubeVideoId,
        playerVars:
        {
          cc_load_policy: videos[i].cc,
          autoplay: 0
        }

      });
    }

    //_this.setiFrameHeight();

    var containerHeight = $('#template_container').outerHeight() + ($('#template_container').outerHeight() * .1);
    parent.updateiFrameHeight("template_" + _this.contentId, containerHeight);
  }

  this.onPlayerReady = function (event) {
     setTimeout(() => {
     }, 1000);
  }

  /// Recognize Hex Value And Format ///
  function formatColor(value){
    if(value.startsWith("#") && value.length > 3){
      return value;
    }
    else{
      return "#"+value;
    }
  }

  ///// Color Validation /////
  function isColor(strColor){
    if(strColor.length<3||strColor==undefined){
      return false;
    }
    else{
      return true;
    }
  }

}

TemplateMaYoutubeSlider.prototype = Object.create(TemplateMaBase.prototype);