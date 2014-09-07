<?php

/*======================================================================*\
|| #################################################################### ||
|| # Utopia Angel: Forum Agent Custom API - PHP sample code           # ||
|| # Author: Brother Green (Tsahi Chitin)                             # ||
|| # Created: 5 June 2004                                             # ||
|| # ---------------------------------------------------------------- # ||
|| # All code in this file is Copyright (C) 2004 by its author(s),    # ||
|| # Brother Green and Utopia Temple. This file may be redistributed  # ||
|| # only in its original form and in its full package.               # ||
|| # ---------------------------------------------------------------- # ||
|| # Make sure to read "forum-agent-api.txt" before using this file!  # ||
|| # ---------------------------------------------------------------- # ||
|| # IMPORTANT: Check for the latest version of this file!            # ||
|| #            http://www.utopiatemple.com/forum-agent-api.zip       # ||
|| #################################################################### ||
\*======================================================================*/


  // These constants can be changed
  define ("API_ENGINE_VERSION", 1);     // http://forums.utopiatemple.com/showthread.php?threadid=50765
  define ("MINIMUM_AGENT_VERSION", 19); // http://forums.utopiatemple.com/showthread.php?threadid=39913
  define ("BULK_MODE", "yes");          // http://forums.utopiatemple.com/showthread.php?threadid=50766
  
  // These constants should NOT be changed
  define ("FORUM_AGENT_HEADER", "[FORUM AGENT API]"); // Forum Agent Custom API header
  define ("INSTRUCTION_PREFIX", "FORUMAGENT:");       // Forum Agent Instruction Prefix
  define ("RECORD_SEP", chr (30));                    // Record delimiter (ASCII #30)
  define ("GROUP_SEP", chr (29));                     // Group delimiter (ASCII #29)
  define ("NL", "\n");                                // New line character (ASCII #10)
  
  // Do not remove the following line, as it provides protection against HTML injection (if "register_globals" is on)
  unset ($instructions);
  
  // Set your custom forum instructions here.
  // Add as many lines as need in the following format.
  // (The "FORUMAGENT:" prefix should be omitted)
//  $instructions [] = 'alternative_forum_by_location="1:51,War with (1:51)"';
//  $instructions [] = 'post_export_lines_only="yes"';
  
  // Login information variables
  $username       = SafeStripSlashes ($_POST ["username"]);
  $password       = SafeStripSlashes ($_POST ["password"]);
  $forum_name     = SafeStripSlashes ($_POST ["forum_name"]);
  $forum_password = SafeStripSlashes ($_POST ["forum_password"]);
  
  // Alternative login method variable (using cookie)
  $cookie         = SafeStripSlashes ($_COOKIE ["cookie"]);
  
  // Intel information variables if BULK_MODE is disabled
  $province       = SafeStripSlashes ($_POST ["province"]);
  $kingdom        = SafeStripSlashes ($_POST ["kingdom"]);
  $island         = SafeStripSlashes ($_POST ["island"]);
  $title          = SafeStripSlashes ($_POST ["title"]);
  $data           = SafeStripSlashes ($_POST ["data"]);
  
  // Intel information variable if BULK_MODE is enabled
  $bulk_data      = SafeStripSlashes ($_POST ["bulk_data"]);

  function SafeStripSlashes ($buf)
  // "magic_quotes_gpc" is set in the php.ini file.
  // If enabled, PHP automatically escapes (adds slashes to) quotes in all GPC (GET/POST/COOKIE) variables submitted by the user.
  // Since we want to work with the original variables content, we strip the slashes here,
  // BUT MAKE SURE TO ADD SLASHES IN ANY SQL QUERY! (to prevent SQL injection attack)
  {
    return (get_magic_quotes_gpc ()) ? stripslashes ($buf) : $buf;
  }

  function Handshake ()
  {
    global $instructions;
    
    $instructions [] = 'api_engine_version="' . API_ENGINE_VERSION . '"';
    $instructions [] = 'minimum_forum_agent_version="' . MINIMUM_AGENT_VERSION . '"';
    $instructions [] = 'bulk_mode="' . BULK_MODE . '"';
    
    echo FORUM_AGENT_HEADER;                                               // Print the Forum Agent identification header
    echo INSTRUCTION_PREFIX . implode (INSTRUCTION_PREFIX, $instructions); // Print the Forum Agent Instructions
  }

  function LoginUser ()
  // Here you should check that the $username and $password are valid and have permission to post in your application.
  // Additionally, you can check for $forum_name and $forum_password.
  // $username and $forum_name will always have a value, while $password and $forum_password are optional.
  {
    global $username, $password, $forum_name, $forum_password;
    
    // If login fails, set a custom error message that will appear in Utopia Angel
    // A few examples follow (commented):
//    $error_message = "Invalid username/password combination!";
//    $error_message = "You do not have permission to post in this forum!";
//    $error_message = "Missing or invalid forum password!";
    
    if ($error_message)
      die ("-LOGIN $error_message");
    
    // If login is successful, you may set a cookie at this stage, which will be returned by the Forum Agent
    // instead of $username, $password, $forum_name and $forum_password in the next HTTP calls.
    // If you choose this method, add an additional check for the cookie in the step above!
    if (!$error_message)
    {
//      setcookie ("cookie", "cookie_value"); // Customize these values, or comment line if unneeded
      echo "+LOGIN" . NL;
    }
  }

  function PostIntel ()
  {
    global $province, $kingdom, $island, $forum_name, $title, $data, $bulk_data;
    
    // Since this script supports BULK_MODE, let's convert single-input to bulk-input format
    // (can be safely removed if you enable BULK_MODE)
    if (!$bulk_data)
      $bulk_data = 0 . RECORD_SEP . $province . RECORD_SEP . $kingdom . RECORD_SEP . $island . RECORD_SEP . $forum_name . RECORD_SEP . $title . RECORD_SEP . $data;
    
    // Process bulk-mode input
    $records = explode (GROUP_SEP, $bulk_data);           // Split groups into records
    foreach ($records as $record_data)                    // Iterate records
    {
      $record_array = explode (RECORD_SEP, $record_data); // Split record elements into array
      $index = array_shift ($record_array);               // The index of the record (used for the result output)
      if (is_numeric ($index))                            // Basic index validation
      {
        $province = array_shift ($record_array);          // The province name (without location)
        $kingdom = array_shift ($record_array);           // The kingdom number
        $island = array_shift ($record_array);            // The island number
        $forum_name = array_shift ($record_array);        // The name of the forum (based on "alternative_forum_by_location" instruction)
        $title = array_shift ($record_array);             // The subject of the post
        $data = array_shift ($record_array);              // The body of the post
        // Additional fields may be added here if supported in a future version
        
        // ------------------------------------------------- \\
        // Here you may process the data in any way you wish \\
        // ------------------------------------------------- \\
        
        // Here you can store the values in the database, based on the descriptions provided above.
        // For example:
        /*
        $query  = "INSERT INTO table_name (provinceid, province, kingdom, island, forum_name, title, data) VALUES ";
        $query .= "(null, ";                              // auto_increment index field
        $query .= "'" . addslashes ($province) . "', ";   // The province name (without location)
        $query .= intval ($kingdom) . ", ";               // The kingdom number
        $query .= intval ($island) . ", ";                // The island number
        $query .= "'" . addslashes ($forum_name) . "', "; // The name of the forum (based on "alternative_forum_by_location" instruction)
        $query .= "'" . addslashes ($title) . "', ";      // The subject of the post
        $query .= "'" . addslashes ($data) . "')";        // The body of the post
        mysql_query ($query);
        */
        
        // If the item was stored successfully, you should tag it as successful.
        // Otherwise, you may specify the exact error message to be displayed (optional).
        // Notice: Returning a negative response will keep the item in the client's queue,
        // resulting in this item being resent over and over again in subsequent sessions,
        // until a positive response is returned. Therefore, if you are rejecting a specific
        // intel and don't want it to be resent, you should return a positive response.
        $error = mysql_error ();
        if (!$error)
          $result [] = "+$index";
        else
          $result [] = "-$index Error storing in database";
      }
    }
    
    if (is_array ($result))
      echo implode (NL, $result) . NL;
  }

  // ------ \\  
  //  MAIN  \\  
  // ------ \\  
  
  if (!$username && !$cookie)
    Handshake (); // STEP #1: HANDSHAKE
  else
  {
    LoginUser (); // STEP #2: LOGIN
    PostIntel (); // STEP #3: POST INTEL
  }

?>