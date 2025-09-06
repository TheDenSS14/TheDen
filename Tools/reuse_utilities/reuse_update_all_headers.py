from tqdm import tqdm
from concurrent.futures import ThreadPoolExecutor, as_completed
from reuse_helperfunctions import get_all_allowed_files, process_file, MAX_WORKERS

def main(filepaths, max_threads):
    """
    given a list of filepaths, updates headers on all files in the codebase
    :param filepaths: list of normalized filepaths
    :param max_threads: thread pool size
    :return: None
    """
    with tqdm(total=len(filepaths), desc="Processing files", position=0) as overall_bar:
        with ThreadPoolExecutor(max_workers=max_threads) as executor:
            futures = {executor.submit(process_file, filepath, thread_safe=True): filepath for filepath in filepaths}

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
                overall_bar.update(1)

if __name__ == "__main__":
    filepaths = get_all_allowed_files()
    main(filepaths, max_threads=MAX_WORKERS)