<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Devolio</title>
    <link rel="stylesheet" href="css/bootstrap.css">
    <link rel="stylesheet" href="prism/prism.css">
    <script src="prism/prism.js"></script>
    <link rel="stylesheet" href="styles.css">
    <script src="main.js"></script>
    <link rel="icon" type="image/x-icon" href="assets/favicon.ico">
</head>
<body style="background-color: #070707; color: white">
<br>
<br>
<br>
<div style="text-align: center;padding: 40px;margin-left: 15%;margin-right: 15%;">
    Hello, world! I'm <span class="gradient">Jackson Gray</span>, a 20-year-old current living in Chicago.
    My aspirations are firmly set on becoming a software engineer, and I'm always eager to get closer to this goal.
    I've enrolled in programming courses at Columbia College Chicago, completed CS50x by Harvard, and undertaken numerous passion projects, continually learning through hands-on experience.
    <br><br>
    While I might see myself as typically boring and uninteresting, code never fails to spark my creativity in ways I didn't think I was capable of.
    My current toolset includes a mix of <span class="gradient">C, C#, C++, HTML, CSS, and PHP</span>, with hands-on experience creating projects and finding solutions in each of these languages.
    I genuinely believe that technology can be whatever you want it to be and has the power to change the world.
    <br><br>
    This webpage serves as a showcase for my skills and knowledge as I continue to grow in the field of software engineering.
    Below, you can find personal projects, schoolwork, and other pieces of my work. All of my code is available on my <a href="https://github.com/jgray08/" target="_blank" class="link">GitHub</a>.
</div>

<?php
$directory = 'Projects';
$files = scandir($directory);
$files = array_diff($files, array('.', '..'));
$filteredFiles = array_filter($files, function($file) {
    return pathinfo($file, PATHINFO_EXTENSION) !== 'txt';
});
?>

<div class="nav">
    <?php foreach ($filteredFiles as $file): ?>
        <?php
        $filename = pathinfo($file, PATHINFO_FILENAME);
        $link = '#' . strtolower(str_replace(' ', '', $filename));
        $ext = pathinfo($file, PATHINFO_EXTENSION);
        ?>
        <a href="<?php echo $link; ?>"><?php echo $filename . '.' . $ext; ?></a>
    <?php endforeach; ?>
</div>

<?php foreach ($filteredFiles as $file):
    $filename = pathinfo($file, PATHINFO_FILENAME);
    $ext = pathinfo($file, PATHINFO_EXTENSION);
    $lang = pathinfo($file, PATHINFO_EXTENSION);
    $languages = array("cs"=>"csharp", "cpp"=>"cpp", "c"=>"c", "html"=>"html", "php"=>"php");
    $lang = $languages[$ext];
    $descriptor = "Looks like I forgot to add the description for this code!";
    if (file_exists($directory . '/' . $filename . '.txt')) {
        $descriptor = file_get_contents($directory . '/' . $filename . '.txt');
    }
    $fileContents = file_get_contents($directory . '/' . $file);
    ?>
    <div id="<?php echo strtolower(str_replace(' ', '', $filename))?>" class="section">
        <h2 class="gradient"><?php echo $filename . '.' . $ext ?></h2>
        <p><?php echo $descriptor ?></p>
        <pre><code class="<?php echo 'language-' . $lang?>">
            <?php echo htmlspecialchars($fileContents); ?>
        </code></pre>
    </div>
<?php endforeach; ?>
<div class="foot">Programmed with &hearts; by Jackson Gray</div>
</body>
</html>