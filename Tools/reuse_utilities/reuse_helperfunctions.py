# SPDX-FileCopyrightText: 2025 Aiden
# SPDX-FileCopyrightText: 2025 MajorMoth

import os
import subprocess
import re
import io
import glob
# made with python 3.12.9
# this script should work perfectly fine both on windows and linux, as paths are handled with os.normpath

# Dictionary mapping file extensions to comment styles
# Format: {extension: (prefix, suffix)}
# If suffix is None, it's a single-line comment style
COMMENT_STYLES = {
    # C-style single-line comments
    ".cs": ("//", None),
    ".js": ("//", None),
    ".ts": ("//", None),
    ".jsx": ("//", None),
    ".tsx": ("//", None),
    ".c": ("//", None),
    ".cpp": ("//", None),
    ".cc": ("//", None),
    ".h": ("//", None),
    ".hpp": ("//", None),
    ".java": ("//", None),
    ".scala": ("//", None),
    ".kt": ("//", None),
    ".swift": ("//", None),
    ".go": ("//", None),
    ".rs": ("//", None),
    ".dart": ("//", None),
    ".groovy": ("//", None),
    ".php": ("//", None),

    # Hash-style single-line comments
    ".yaml": ("#", None),
    ".yml": ("#", None),
    ".ftl": ("#", None),
    ".py": ("#", None),
    ".rb": ("#", None),
    ".pl": ("#", None),
    ".pm": ("#", None),
    ".sh": ("#", None),
    ".bash": ("#", None),
    ".zsh": ("#", None),
    ".fish": ("#", None),
    ".ps1": ("#", None),
    ".r": ("#", None),
    ".rmd": ("#", None),
    ".jl": ("#", None),  # Julia
    ".tcl": ("#", None),
    ".perl": ("#", None),
    ".conf": ("#", None),
    ".toml": ("#", None),
    ".ini": ("#", None),
    ".cfg": ("#", None),
    ".gitignore": ("#", None),
    ".dockerignore": ("#", None),

    # Other single-line comment styles
    ".bat": ("REM", None),
    ".cmd": ("REM", None),
    ".vb": ("'", None),
    ".vbs": ("'", None),
    ".bas": ("'", None),
    ".asm": (";", None),
    ".s": (";", None),  # Assembly
    ".lisp": (";", None),
    ".clj": (";", None),  # Clojure
    #".f": ("!", None),   # Fortran - this need special handling per file
    #".f90": ("!", None), # Fortran - this need special handling per file
    #".m": ("%", None),   # MATLAB/Octave - this need special handling per file
    #".sql": ("--", None),# this needs special handling per file
    ".ada": ("--", None),
    ".adb": ("--", None),
    ".ads": ("--", None),
    ".hs": ("--", None), # Haskell
    ".lhs": ("--", None),
    ".lua": ("--", None),

    # Multi-line comment styles
    ".xaml": ("<!--", "-->"),
    ".xml": ("<!--", "-->"),
    ".html": ("<!--", "-->"),
    ".htm": ("<!--", "-->"),
    ".svg": ("<!--", "-->"),
    ".css": ("/*", "*/"),
    ".scss": ("/*", "*/"),
    ".sass": ("/*", "*/"),
    ".less": ("/*", "*/"),
    ".md": ("<!--", "-->"),
    ".markdown": ("<!--", "-->"),
}

# all the licenses we use currently
LICENSE_CONFIG = {
    "mit": {"id": "MIT", "path": "LICENSES/MIT.txt"},
    "agpl": {"id": "AGPL-3.0-or-later", "path": "LICENSES/AGPLv3.txt"},
    "mpl": {"id": "MPL-2.0", "path": "LICENSES/MPL-2.0.txt"},
}

# the default license assumed when the PR body does not contain any
DEFAULT_LICENSE_LABEL = "agpl"

IGNORED_AUTHORS = {
    "PJBot",
    "github-actions[bot]",
    "GoobBot",
    "GoobBot[bot]",
    "GoobBot [bot]",
    "TheDen-Bot",
}

REPO_PATH = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))

# these start at the repo's path. that is, an empty string here would make literally everything be ignored since the check also includes subdirectories.
IGNORED_DIRECTORIES: (str,) = (
    "bin",
    "RobustToolbox"
)

IGNORED_DIRECTORIES = frozenset(os.path.abspath(os.path.join(REPO_PATH, ignored)) for ignored in IGNORED_DIRECTORIES)

ENV = os.environ.copy()
ENV["PATH"] = "/usr/bin:/bin:/usr/local/bin"  # Linux-only paths

ANSI_COLORS = {
    "black":   "\033[30",
    "red":     "\033[31",
    "green":   "\033[32",
    "yellow":  "\033[33",
    "blue":    "\033[34",
    "magenta": "\033[35",
    "cyan":    "\033[36",
    "white":   "\033[37",
    "reset":   "\033[0",
    "doing":    "\033[36",
    "command": "\033[34",
}

THREAD_SAFETY_DEFAULT = False

MAX_WORKERS = os.cpu_count() or 1

def colout(text:str, color:str = "white"):
    """
    colors console output
    :param text: original string we want to color
    :param color: string, should match one option in ANSI_COLORS or it's going to fail
    :return: modified string with ANSI escape sequences that should show up colored in the terminal
    """
    return f"\033[{ANSI_COLORS[color.lower()]}m{text}\033[0m"

def run_git_command(command, cwd=REPO_PATH, check=True, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    runs a git command and captures its output
    :param command: the command to run, as a string
    :param cwd: the location the command should be run in, as a string
    :param check: whether to check for errors, bool
    :param thread_safe: determines the console output behaviour, bool
    :return: command output as a string and optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    try:
        result = subprocess.run(
            command,
            env=ENV,
            capture_output=True,
            text=True,
            check=check,
            cwd=cwd,
            encoding='utf-8',
            errors='ignore'
        )
        if not thread_safe:
            return result.stdout.strip()
        else:
            return buffer, result.stdout.strip()
    except subprocess.CalledProcessError as e:
        if check:
            thread_safe_print(f"Error running git command {' '.join(command)}: {e.stderr}", "red")
        if not thread_safe:
            return None
        else:
            return buffer, None
    except FileNotFoundError:
        thread_safe_print("FATAL: 'git' command not found. Make sure git is installed and in your PATH.", "red")
        if not thread_safe:
            return None
        else:
            return buffer, None

def get_license_from_commit(commit_sha, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    grabs a license from the pr body attached to the merge commit message
    falls back to the default license if it doesn't find one
    :param commit_sha: the full SHA hash of a merge commit, as a string
    :param thread_safe: determines the console output behaviour, bool
    :return: the license found or the default as a string, and optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    thread_safe_print(f"Looking for license in commit {commit_sha}:", "doing")
    command = ["git", "show", "-s", "--format=%B", commit_sha]
    thread_safe_print(f"Using command: {' '.join(command)}", "command")
    if not thread_safe:
        git_output = run_git_command(command)
    else:
        git_command_buffer, git_output = run_git_command(command, thread_safe=True)
        git_command_buffer.seek(0)
        buffer.write(git_command_buffer.read())
    match = re.search(r"LICENSE: (\w+)", git_output)
    if match:
        found_license = match.group(1).lower() # we need to make it lower otherwise it won't match anything in LICENSE_CONFIG
        if found_license not in LICENSE_CONFIG:
            thread_safe_print(f"License {found_license} is not in LICENSE_CONFIG, falling back to {DEFAULT_LICENSE_LABEL}","red")
            if not thread_safe:
                return DEFAULT_LICENSE_LABEL
            else:
                return buffer, DEFAULT_LICENSE_LABEL
        thread_safe_print(f"Found license in commit {commit_sha}:", "green")
        thread_safe_print(found_license)
        if not thread_safe:
            return found_license
        else:
            return buffer, found_license
    else:
        thread_safe_print(f"Did not find a license in the commit, falling back to: {DEFAULT_LICENSE_LABEL}", "yellow")
        if not thread_safe:
            return DEFAULT_LICENSE_LABEL
        else:
            return buffer, DEFAULT_LICENSE_LABEL


def get_files_from_commit(commit_sha, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    returns a list of paths that a given merge commit added or modified (we don't care about deletions)
    :param commit_sha: the full SHA hash of a merge commit, as a string
    :param thread_safe: determines the console output behaviour, bool
    :return: a list of strings like [str, str] that are the normalized paths to the files affected by the merge commit, and optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    # Get all files added or modified by a merge/squash merge commit
    thread_safe_print(f"Looking for all files added, modified or renamed by commit {commit_sha}:", "doing")
    command = ["git", "diff", "--name-status", "-M", f"{commit_sha}^1", commit_sha]
    thread_safe_print(f"Using command: {' '.join(command)}", "command")

    if not thread_safe:
        git_output = run_git_command(command)
    else:
        git_command_buffer, git_output = run_git_command(command, thread_safe=True)
        git_command_buffer.seek(0)
        buffer.write(git_command_buffer.read())

    git_output = [  # this removes any files from the list, that were removed by the commit
        parts[2] if parts[0].startswith("R") else parts[1]
        for parts in (x.split("\t") for x in git_output.splitlines() if x[0] != "D")
    ]

    thread_safe_print(f"The following files have been added, modified or renamed by commit {commit_sha}:", "green")
    thread_safe_print(f"{"\n".join(git_output)}")
    normalized_paths = [os.path.normpath(line) for line in git_output if line.strip()]
    if not thread_safe:
        return normalized_paths
    else:
        return buffer, normalized_paths

def get_authors_from_file(filepath, follow=True, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    gets all historical authors from a file using git log
    :param filepath: path to the file (relative), should be normalised, string
    :param follow: whether to use the --follow flag when using git log, bool
    :param thread_safe: determines the console output behaviour, bool
    :return: returns a list lists of raw strings like [[yyyy, author],[yyyy, author]], and optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    thread_safe_print(f"Looking for historical authors in {filepath}:", "doing")
    command = ["git", "log", "--pretty=format:%ad|%an", "--date=short", "--reverse", "--date-order"] # reverse, because it makes more sense to add an author to the end of the header when we update it
    if follow:
        command.extend(("--follow", filepath))
    else:
        command.append(filepath)
    thread_safe_print(f"Using command: {' '.join(command)}", "command")
    if not thread_safe:
        git_output = run_git_command(command)
    else:
        git_command_buffer, git_output = run_git_command(command, thread_safe=True)
        git_command_buffer.seek(0)
        buffer.write(git_command_buffer.read())
    git_output = list(line.split("|") for line in git_output.splitlines())
    thread_safe_print(f"Found the following historical authors in {filepath}:", "green")
    thread_safe_print(f"{", ".join(git_output[i][1] for i in range(len(git_output)))}")
    for i in range(len(git_output)): # trim the date to just the year (this is due to copyright, and being in line with existing headers), hopefully this won't be problematic in the year 10000
        git_output[i][0] = git_output[i][0][0:4]
    if not thread_safe:
        return git_output
    else:
        return buffer, git_output

def parse_existing_header(content, comment_style:(str,), filepath, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    parses an existing header on a file and returns a 3 element tuple composed of:
    1. the list of authors (structured like get_authors_from_file)
    2. the license id's found in a set
    3. a string that is the header found on the file
    :param content: the file content, as a string
    :param comment_style: comment style from COMMENT_STYLES based on COMMENT_STYLES.get(ext)
    :param filepath: path to the file (relative), should be normalised, string
    :param thread_safe: determines the console output behaviour, bool
    :return: see function description + optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    file_lines = content.splitlines()
    prefix, suffix = comment_style
    thread_safe_print(f"Checking if a header exists in {filepath}:", "doing") # new files won't have headers, or sometimes modified files might have theirs stripped
    if suffix:
        if file_lines[0].strip() != prefix:
            thread_safe_print(f"Did not find a header in {filepath}", "yellow")
            if not thread_safe:
                return [], {}, ""
            else:
                return buffer, [], {}, ""
    else:
        if file_lines[0].strip().split(" ")[0] != prefix:
            thread_safe_print(f"Did not find a header in {filepath}", "yellow")
            if not thread_safe:
                return [], {}, ""
            else:
                return buffer, [], {}, ""
    thread_safe_print(f"Found a header in {filepath}", "green")
    header_lines = []
    # we capture the existing header to then make it easier to update it
    # otherwise we'd need to once again iterate over the file and find the header when we get to actually writing to the file

    if suffix:
        for line in file_lines:
            header_lines.append(line)
            if line == suffix:
                break
    else:
        for line in file_lines:
            if not line or line == "\n":
                break
            if line.split(" ")[0] != prefix:
                header_lines.append(line)
                break
            header_lines.append(line)

    thread_safe_print(f"Looking for licenses in {filepath}:", "doing")
    match = re.search(r"SPDX-License-Identifier: (.+)", "\n".join(header_lines)) # find licenses
    if match:
        found_licenses = set(match.group(1).split(" AND "))
        thread_safe_print(f"Found the following licenses in {filepath}:", "green")
        thread_safe_print(", ".join(found_licenses))
    else:
        thread_safe_print(f"Did not find any licenses in {filepath}.", "yellow")
        found_licenses = {}

    thread_safe_print(f"Looking for existing authors in {filepath}:", "doing")
    authors: [[str, str]] = []
    if suffix:
        copyright_regex = re.compile(f"SPDX-FileCopyrightText: (\\d{{4}}) (.+?)(?: <([^>]+)>)?$")
        for line in header_lines:
            match = re.search(copyright_regex, line)
            if match:
                authors.append([match.group(1), match.group(2)])
                thread_safe_print(f"{line}{colout(f" -> {" ".join(authors[-1])}", "green")}")
    else:
        copyright_regex = re.compile(rf"^{re.escape(prefix)} SPDX-FileCopyrightText: (\d{{4}}) (.+?)(?: <([^>]+)>)?$")
        for line in header_lines:
            match = re.search(copyright_regex, line)
            if match:
                authors.append([match.group(1), match.group(2)])
                thread_safe_print(f"{line}{colout(f" -> {" ".join(authors[-1])}", "green")}")
    if authors:
        thread_safe_print(f"Found the following existing authors in {filepath}: ", "green")
        thread_safe_print(f"{", ".join(authors[i][1] for i in range(len(authors)))}")
    else:
        thread_safe_print(f"Did not find any existing authors in {filepath}", "red")
    if not thread_safe:
        return authors, found_licenses, "\n".join(header_lines)
    else:
        return buffer, authors, found_licenses, "\n".join(header_lines)

def process_author_list(raw_authors):
    """
    takes the output from get_authors_from_git or parse_existing_header and returns a list of strings where each string is "yyyy author"
    this function is to be used on the final list of authors, right before feeding it to create_header
    :param raw_authors: the authors, structured like get_authors_from_file
    :return: the list of authors, sorted and properly formatted, as a list of strings
    """
    authors_list: [str] = []
    seen_authors_set = set()
    for author_date, author_name in raw_authors:
        if author_name in seen_authors_set:
            continue
        if author_name in IGNORED_AUTHORS:
            continue
        if not author_name:
            continue
        if not author_date:
            continue

        seen_authors_set.add(author_name)
        authors_list.append(f"{author_date} {author_name}")

    authors_list.sort()
    return authors_list

def create_header(authors, license_id: str, comment_style:(str,), filepath, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    creates a header based on a list of authors from process_author_list, the provided license id, and the comment style of the file. filepath is used only for logging purposes
    :param authors: the processed, ready-to-use author list you get from process_author_list
    :param license_id: the license identifier, what you get from doing something like LICENSE_CONFIG[DEFAULT_LICENSE_LABEL]["id"]
    :param comment_style: comment style from COMMENT_STYLES, based on COMMENT_STYLES.get(ext)
    :param filepath: path to the file (relative), should be normalised, string
    :param thread_safe: determines the console output behaviour, bool
    :return: a string that is the new header, and optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    header = []
    prefix, suffix = comment_style
    if suffix: # multi line comment style
        thread_safe_print(f"Creating a multiline-style header for {filepath}:", "doing")
        header.append(prefix)
        for line in authors:
            header.append(f"SPDX-FileCopyrightText: {line}")
        header.append(f"\nSPDX-License-Identifier: {license_id}\n{suffix}")
        thread_safe_print(f"Multiline-style header for {filepath} created successfully.", "green")
        if not thread_safe:
            return "\n".join(header)
        else:
            return buffer, "\n".join(header)
    else: # single line comment style
        thread_safe_print(f"Creating a singleline-style header for {filepath}:", "doing")
        for line in authors:
            header.append(f"{prefix} SPDX-FileCopyrightText: {line}")
        header.append(f"{prefix}\n{prefix} SPDX-License-Identifier: {license_id}")
        thread_safe_print(f"Singleline-style header for {filepath} created successfully.", "green")
        if not thread_safe:
            return "\n".join(header)
        else:
            return buffer, "\n".join(header)

def prepend_header(filepath, new_header, existing_header, thread_safe=THREAD_SAFETY_DEFAULT):
    """
    this function does the actual writing to the file (only if it needs to, that is, only if the header has changed at all)
    :param filepath: path to the file (relative), should be normalised, string
    :param new_header: the new header created by create_header, string
    :param existing_header: the existing header, if found by parse_existing_header, as a string
    :param thread_safe: determines the console output behaviour, bool
    :return: None or optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    with open(os.path.join(REPO_PATH, filepath), "r", encoding="utf-8") as file:
        content = file.read()

    if existing_header:
        thread_safe_print(f"Replacing existing header in {filepath}:", "doing")
        new_content = content.replace(existing_header, new_header)
    else:
        thread_safe_print(f"Prepending a header to {filepath}:", "doing")

        _, ext = os.path.splitext(filepath)
        insert_after = 0
        lines = content.splitlines(keepends=True)

        if ext in [".sh", ".bash", ".zsh", ".fish", ".ps1"]:
            # keep shebang first if present
            thread_safe_print(f"Possible shebang, handling...", "yellow")
            if lines and lines[0].startswith("#!"):
                thread_safe_print(f"...shebang detected.", "yellow")
                insert_after = 1

        elif ext in [".bat", ".cmd"]:
            # keep @echo off first if present
            thread_safe_print(f"Possible @echo off, handling...", "yellow")
            if lines and lines[0].strip().lower().startswith("@echo off"):
                insert_after = 1

        elif ext in [".xml", ".xaml", ".svg", ".html", ".htm"]:
            # keep XML declaration or DOCTYPE first
            thread_safe_print(f"Possible document declaration, handling...", "yellow")
            if lines and (lines[0].startswith("<?xml") or lines[0].lower().startswith("<!doctype")):
                thread_safe_print(f"...document declaration detected.", "yellow")
                insert_after = 1

        elif ext in [".md", ".markdown"]:
            # handle YAML/TOML front matter
            thread_safe_print(f"Possible front matter, handling...", "yellow")
            if lines and (lines[0].startswith("---") or lines[0].startswith("+++")):
                thread_safe_print(f"...front matter detected.", "yellow")
                # find closing marker
                for i in range(1, len(lines)):
                    if lines[i].startswith(lines[0].strip()):
                        insert_after = i + 1
                        break

        header_str = new_header.rstrip("\r\n")

        # Prepare the content after insertion point, and remove any extra leading newlines in the rest of the content
        rest = "".join(lines[insert_after:]).lstrip("\r\n")

        # Combine header + one newline + content
        new_content = "".join(lines[:insert_after]) + header_str + "\n\n" + rest

    if new_content == content:
        thread_safe_print(f"Header in {filepath} did not have to be modified.", "green")
        if not thread_safe:
            return
        else:
            return buffer

    with open(os.path.join(REPO_PATH, filepath), "w", encoding="utf-8") as file:
        file.write(new_content)
    thread_safe_print(f"Successfully updated header in {filepath}", "green")

    if not thread_safe:
        return
    else:
        return buffer

def process_file(filepath, given_license:str = "", thread_safe=THREAD_SAFETY_DEFAULT):
    """
    designed for a threadpool - this uses all the other helper functions to actually process a single file according to the proper flow
    :param filepath: path to the file (relative), should be normalised, string
    :param given_license: this is an optional parameter that will override the default license specified in the script configuration
    :param thread_safe: determines the console output behaviour, bool
    :return: none, or optionally a buffer as a StringIO object
    """
    if thread_safe:
        buffer = io.StringIO()
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s, color), file=buffer)
    else:
        def thread_safe_print(s:str, color:str = "white"):
            print(colout(s,color))

    _, ext = os.path.splitext(filepath)
    comment_style = COMMENT_STYLES.get(ext)
    if not comment_style:
        thread_safe_print(f"Unsupported file type: {ext}", "red")
        if not thread_safe:
            return
        else:
            return buffer
    prefix, suffix = comment_style

    with open(os.path.join(REPO_PATH, filepath), "r", encoding="utf-8") as file:
        content = file.read()
    if not thread_safe:
        existing_authors, licenses, existing_header = parse_existing_header(content, comment_style, filepath)
        historical_authors = get_authors_from_file(filepath)
        combined_authors = process_author_list(existing_authors + historical_authors)
    else:
        parse_existing_header_buffer, existing_authors, licenses, existing_header = parse_existing_header(content, comment_style, filepath, thread_safe=True)
        parse_existing_header_buffer.seek(0)
        buffer.write(parse_existing_header_buffer.read())
        authors_from_file_buffer, historical_authors = get_authors_from_file(filepath, thread_safe=True)
        authors_from_file_buffer.seek(0)
        buffer.write(authors_from_file_buffer.read())
        combined_authors = process_author_list(existing_authors + historical_authors)

    # deal with licensing
    # this SHOULD result in at least one license in every possible case
    if given_license:
        if not licenses:
            licenses = {LICENSE_CONFIG[given_license]["id"]}
        elif given_license == LICENSE_CONFIG["mit"]["id"]:
            if LICENSE_CONFIG["mpl"]["id"] not in licenses:
                licenses.add(LICENSE_CONFIG[given_license]["id"])
    else:
        if not licenses:
            licenses = {LICENSE_CONFIG[DEFAULT_LICENSE_LABEL]["id"]}

    if not thread_safe:
        new_header = create_header(
            combined_authors,
            " AND ".join(licenses),
            comment_style,
            filepath)
    else:
        create_header_buffer, new_header = create_header(
            combined_authors,
            " AND ".join(licenses),
            comment_style,
            filepath,
            thread_safe=True)
        create_header_buffer.seek(0)
        buffer.write(create_header_buffer.read())

    if new_header:
        thread_safe_print(new_header)
    else:
        thread_safe_print(f"Failed to create a new header for {filepath}", "red")
        if not thread_safe:
            return
        else:
            return buffer

    if not thread_safe:
        return
    else:
        prepend_header_buffer = prepend_header(filepath, new_header, existing_header, thread_safe=True)
        prepend_header_buffer.seek(0)
        buffer.write(prepend_header_buffer.read())
        return buffer

def is_ignored(filepath: str) -> bool:
    """
    simple helper to check if a directory is ignored
    :param filepath: path to the file (relative), should be normalised, string
    :return: bool
    """
    if os.path.splitext(filepath)[1] not in COMMENT_STYLES:
        return True

    return any(
        filepath == ignored or filepath.startswith(ignored + os.sep)
        for ignored in IGNORED_DIRECTORIES
    )

def get_all_allowed_files(path = REPO_PATH) -> {str}:
    """
    helper that grabs all the files in the repo
    :param path: optional override if you're running this script from somewhere else than the repo
    :return: a set of normalised filepaths
    """
    return set(os.path.normpath(f) for f in glob.iglob(os.path.normpath(f"{path}/**"), recursive=True) if os.path.isfile(f) and not is_ignored(f))
