"""
Problem

The GC-content of a DNA string is given by the percentage of symbols in the string that are 'C' or 'G'. For example, the GC-content of "AGCTATAG" is 37.5%. Note that the reverse complement of any DNA string has the same GC-content.

DNA strings must be labeled when they are consolidated into a database. A commonly used method of string labeling is called FASTA format. In this format, the string is introduced by a line that begins with '>', followed by some labeling information. Subsequent lines contain the string itself; the first line to begin with '>' indicates the label of the next string.

In Rosalind's implementation, a string in FASTA format will be labeled by the ID "Rosalind_xxxx", where "xxxx" denotes a four-digit code between 0000 and 9999.

Given: At most 10 DNA strings in FASTA format (of length at most 1 kbp each).

Return: The ID of the string having the highest GC-content, followed by the GC-content of that string. Rosalind allows for a default error of 0.001 in all decimal answers unless otherwise stated; please see the note on absolute error below.
"""

import re

def compute_gc_content(file_path):
	with open(file_path, "r") as f:
		data = parse_fasta_file(f)

	return 0

def parse_fasta_file(file):
	fasta_ids = []
	gc_strings = []
	concat_gc_list = []
	ratio_gc_list = []

	for line in file:
		line = line.rstrip("\n")

		if line.startswith(">"):
			fasta_ids.append(line)

		if not line.startswith(">"):
			gc_strings.append(line)

	new_list = split_list(gc_strings, (len(gc_strings)/2))

	for list_item in new_list:
		concat_gc_list.append(list_item[0] + list_item[1])

	for gc_list_item in concat_gc_list:
		ratio_gc_list.append(get_ratio(gc_list_item))
		
	gc_percentage_list = list(map(lambda x: x * 100, ratio_gc_list))

	fasta_ids_newline = list(map(lambda x: x + '\n', fasta_ids))
	final_gc_list = [a + b  for a, b in zip(fasta_ids_newline, concat_gc_list)]

	return final_gc_list

def get_ratio(gc_list_item):
	gc_count = 0
	total_base_count = 0
	gc_ratio = 0

	gc_count += len(re.findall("[GC]", gc_list_item))
	total_base_count += len(re.findall("[GCTA]", gc_list_item))

	gc_ratio = float(gc_count) / total_base_count
	return gc_ratio

def split_list(alist, wanted_parts=1):
    length = len(alist)
    return [ alist[i*length // wanted_parts : (i+1)*length // wanted_parts] 
             for i in range(wanted_parts) ]

if __name__ == "__main__":

	result = compute_gc_content("working.txt")
	print str(result)

	# new dataset output
	f = open('workfile.txt', 'w')
	f.write(str(result))
