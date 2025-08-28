from reuse_helperfunctions import get_files_from_commit, get_license_from_commit, process_file, MAX_WORKERS, colout
import argparse
from tqdm import tqdm
from concurrent.futures import ThreadPoolExecutor, as_completed

def main(filepaths, license, max_threads):
    """
    given a list of filepaths, updates headers on all files in it
    :param filepaths: list of normalized filepaths
    :param max_threads: thread pool size
    :return: None
    """
    with tqdm(disable=True) as _:
        with ThreadPoolExecutor(max_workers=max_threads) as executor:
            futures = {executor.submit(process_file, filepath, license, thread_safe=True): filepath for filepath in filepaths}

            for future in as_completed(futures):
                filepath = futures[future]
                try:
                    buffer = future.result()
                    buffer.seek(0)
                    tqdm.write("=" * 5 + f" Start of output for {filepath} " + "=" * 5)
                    tqdm.write(f"{buffer.read()}", end="")
                    tqdm.write("=" * 5 + f" End of output for {filepath} " + "=" * 5)
                    buffer.close()
                except Exception as e:
                    tqdm.write(f"Error processing {filepath}: {e}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Updating headers on files added, modified, or renamed by this commit")
    parser.add_argument(
        "commit_hash",  # positional argument -> required
        type=str,
        help="The git commit hash to process"
    )
    args = parser.parse_args()
    commit = args.commit_hash

    buffer, commit_license = get_license_from_commit(commit, thread_safe=True)
    files_from_commit_buffer, filepaths = get_files_from_commit(commit, thread_safe=True)
    files_from_commit_buffer.seek(0)
    buffer.write(files_from_commit_buffer.read())
    buffer.seek(0)
    print(buffer.read(), end="")
    main(filepaths, commit_license, max_threads=MAX_WORKERS)
    print(colout("^"*76, "finished")) # could be done with shutil but eh who cares
    print(colout(f"Finished updating headers based on: {commit}", "finished"))
