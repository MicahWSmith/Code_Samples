<h2>Navigation</h2>
<ul>
  <li><a href="https://github.com/MicahWSmith/Code_Samples/edit/main/js">JavaScript + PHP</a></li>
  <li><a href="https://github.com/MicahWSmith/Code_Samples/edit/main/cs">C# MVM Pattern</a></li>
  <li><a href="https://github.com/MicahWSmith/Code_Samples/edit/main/cpp">C++ Alphabetical BST</a></li>
</ul>

<h2>Overview</h2>
<p>In this application I take JSON data gathered from the user to display a scrollable web feed of youtube videos for mobile devices. For ease of understanding, here is a snippet from the test data I used which was modeled after the data that would be used in production.</p>

```json
{
  "bg_color": "3d6cdc",
  "header_text": "My Youtube Videos",
  "header_text_font_color":"fafcfe",
  "header_text_font":"Cambria",
  "header_icon": "",
  "initial_display_state":"chevron_no",
  "slide_opacity":"100",
  "slide_bg_color":"",
  "s1_youtube_video_id": "https://www.youtube.com/watch?v=NH9kwje2k8U",
  "s1_video_desc": "What is Apple's M1 chip and it's architecture.",
  "s1_video_desc_font_color":"000000",
  "s1_video_desc_font":"Cambria",  
  "s1_volume": 5,
  "s1_cc": true,
}
```
<p>These values are found under the "content" key of the JSON test data. Styling features chosen by the user are stored such as colors and fonts, as well as the videos they want to show and its properties. The s1 denotes the first "slide" or scrollable video. If it were up to me I would have used an array of videos instead of the s + number prefix, but since I was working with existing data frameworks you will see how I handle this in the code.</p>

***
<p>Moving to the Youtube Slider script file. Below I define a class for the YoutubeVideo objects which I will be manipulating throughout the application.</p>

```javaScript
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
```
<p>I have a YoutubeVideo object array storing the videos for the app.</p>

```JavaScript
  let videos = [];
```

<p>In the init function of the app I call on another function to fill the video array and you will see how I account for the prefix in the JSON data. Given the existing template framework in place I can retrieve the JSON data for a video through _this.contentField. In order to handle the prefix as I am iterating through data, I wrap my data validation and variable setting in eval() which evaluates the string as a JS statement. Once the current video has been set, it is pushed to the array for ease of data handling. The count is also used to set a CSS variable which helps the layout dynamically scale with how many videos are shown</p>

```JavaScript
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
```

<p>To build the body of the application first I get the defined videos, then show them on the page and apply styling with JQuery</p>

```JavaScript
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
    
  }
```

<p>Styling for the app</p>

```css
/* Init of Variables */
      :root {
          --gutter: 5%;
          --maxVideos: 0;
          --slideColor: #ffffff;
          --slideWidth: 100%;
        }
        
        .hs {
          display: grid;
          gap: calc(var(--gutter) / 2);
          grid-template-columns: repeat(var(--maxVideos), calc(var(--slideWidth) - var(--gutter) * 2));
          grid-template-rows: minmax(0px, 1fr);
          overflow-x: scroll;
          scroll-snap-type: x proximity;
          list-style: none;
          padding: 10px;
          margin-top: 0px;
          margin-bottom:0px;
        }
        
        .hs > li, .item{
          overflow: hidden;
          scroll-snap-align: center;
          display: flex;
          flex-direction: column;
          flex-wrap: nowrap;
          background: var(--slideColor);
          border-radius: 8px;
        }

        ::-webkit-scrollbar {
          display: none;
        }
```
