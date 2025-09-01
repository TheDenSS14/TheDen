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
                    tqdm.write(f"Error processing file {filepath}: {e}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Updating headers on files added, modified, or renamed by this commit")
    parser.add_argument(
        "commit_hash",  # positional argument -> required
        type=str,
        help="The git commit hash to process"
    )
    args = parser.parse_args()
    commit = args.commit_hash

    try:
        commit_license = get_license_from_commit(commit)
        filepaths = get_files_from_commit(commit)
        print(colout(processing := f"Processing files from: {commit}", "main"))
        print(colout("-" * len(processing), "main"))
        main(filepaths, commit_license, max_threads=MAX_WORKERS)
        print(colout(finished := f"Finished updating headers based on: {commit}", "main"))
        print(colout("-" * len(finished), "main"))
    except Exception as e:
        print(colout(f"Error processing commit {commit}: {e}", "error"))