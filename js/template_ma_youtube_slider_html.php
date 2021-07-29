<?php
define("GLOBAL_MA_HOST_URL", "http://localhost/ds_widget2/ma/");

if ($fullWidgetText == null) $fullWidgetText = array();
if ($contentId == null) $contentId = 0;
if ($sa == null) $sa = "true";
?>
<!doctype html>
<html lang="en">

<head>
  <meta charset="utf-8">
  <?php echo $metaData; ?>
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <link href="<?php echo GLOBAL_MA_HOST_URL; ?>css/font-awesome.min.css" rel="stylesheet">
  <script src="<?php echo GLOBAL_MA_HOST_URL; ?>js/lib/jquery.js"></script>
  <script src="<?php echo GLOBAL_MA_HOST_URL; ?>js/lib/imagesloaded.pkgd.min.js"></script>
  <script src="<?php echo GLOBAL_MA_HOST_URL; ?>templates/template_ma_base.js?v=<?php echo uniqid(); ?>"></script>
  <script src="https://www.youtube.com/player_api"></script>
  <script src="<?php echo GLOBAL_MA_HOST_URL; ?>templates/template_ma_youtube_slider_html/template_ma_youtube_slider_html.js?v=<?php echo uniqid(); ?>"></script>

  <style>
      
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
  </style>

  <script>
    var fullWidget = <?php echo $fullWidgetText; ?>;
    var contentId = <?php echo $contentId; ?>;
    var isStandAlone = <?php echo $sa; ?>;
    var m_template = undefined;

    $(document).ready(function() {
      if (parent.m_fullWidget != null) {
        fullWidget = parent.m_fullWidget;
      }
      m_template = new TemplateMaYoutubeSlider();
      m_template.initTemplateFullWidget(contentId, fullWidget, isStandAlone);
      m_template.playTemplate();

      window.YT.ready(function() {
        m_template.onYouTubeIframeAPIReady();
      });

    });
  </script>
</head>

<body id="body">
  <div id="template_container">
    <div id="template_header"></div>
    <div id="content">
      <ol id ="youtube" class="hs"></ol>
    </div>
    <div id="template_footer">
      <div id="template_date"></div>
      <div id="template_share" onclick="m_template.shareTemplate()"></div>
    </div>
  </div>
</body>

</html>