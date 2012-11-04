import urllib2
from HTMLParser import HTMLParser
import simplejson as json
import os
import re


class CourseParser(HTMLParser):
    HEADERS = 0
    TABLE_CONTENTS = 1
    FIRST_ROW = 2
    SECOND_ROW = 3
    THIRD_ROW = 4
    NUMBER = 5
    NAME = 6
    UNITS = 7
    PREREQS = 8
    DESCRIPTION = 9
    FOURTH_ROW = 10
    FIFTH_ROW = 11
    SIXTH_ROW = 12
    status = HEADERS
    previous_status = HEADERS

    courses = []
    course = {}

    def __init__(self):
        self.courses = []
        self.course = {}
        self.course['number'] = ""
        HTMLParser.__init__(self)

    def postProcess(self):
        self.course['number'] = re.sub(r'[^\d]', r'', self.course['number'])

    def handle_starttag(self, tag, attrs):
        # print("start: "  + tag+" " +str(self.status))
        if self.status == self.HEADERS:
            if tag == "table":
                self.status = self.TABLE_CONTENTS
        elif self.status == self.TABLE_CONTENTS:
            if tag == "tr":
                if self.previous_status == self.FIRST_ROW:
                    self.status = self.THIRD_ROW
                elif self.previous_status == self.SECOND_ROW:
                    self.status = self.THIRD_ROW
                elif self.previous_status == self.THIRD_ROW:
                    self.status = self.FOURTH_ROW
                elif self.previous_status == self.FOURTH_ROW:
                    self.status = self.FIFTH_ROW
                elif self.previous_status == self.FIFTH_ROW:
                    self.status = self.SIXTH_ROW
                else:
                    self.status = self.FIRST_ROW
                self.previous_status = self.TABLE_CONTENTS
        elif self.status == self.FIRST_ROW:
            if tag == "td":
                if self.previous_status == self.NAME:
                    self.status = self.UNITS
                elif self.previous_status == self.NUMBER:
                    self.status = self.NAME
                else:
                    self.status = self.NUMBER
                self.previous_status = self.FIRST_ROW
        elif self.status == self.SECOND_ROW:
            if tag == "td":
                self.status = self.PREREQS
                self.previous_status = self.SECOND_ROW
        elif self.status == self.THIRD_ROW:
            if tag == "td":
                self.status = self.DESCRIPTION
                self.previous_status = self.THIRD_ROW
        elif self.status == self.DESCRIPTION:
            if tag == "i":
                self.status = self.PREREQS
                self.previous_status = self.SECOND_ROW

    def handle_endtag(self, tag):

        # print("end: " + tag+" " +str(self.status))
        if self.status == self.TABLE_CONTENTS:
            if tag == "table":
                self.status = self.HEADERS
        elif self.status == self.FIRST_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
        elif self.status == self.SECOND_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
        elif self.status == self.THIRD_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
                if(len(self.course) > 0):
                    self.postProcess();
                    self.courses.append(self.course)
                self.course = {}
                self.course['number'] = ""
        elif self.status == self.FOURTH_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
        elif self.status == self.FIFTH_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
        elif self.status == self.SIXTH_ROW:
            if tag == "tr":
                self.previous_status = self.status
                self.status = self.TABLE_CONTENTS
        elif self.previous_status == self.FIRST_ROW:
            if tag == "td":
                self.previous_status = self.status
                self.status = self.FIRST_ROW
        elif self.previous_status == self.SECOND_ROW:
            if tag == "td":
                self.previous_status = self.status
                self.status = self.SECOND_ROW
        elif self.previous_status == self.THIRD_ROW:
            if tag == "td":
                self.previous_status = self.status
                self.status = self.THIRD_ROW

    def handle_data(self, data):
        #print("data: " + data+" " +str(self.status))
        if self.status == self.NUMBER:
            self.course['number'] = self.course['number'] + data
        elif self.status == self.NAME:
            self.course['name'] = data
        elif self.status == self.UNITS:
            split = data.split(" - ")
            self.course['units'] = split[0]
            if "Fall" in data or "Spring" in data or "Summer" in data:
                self.course['fall'] = "Fall" in data
                self.course['spring'] = "Spring" in data
                self.course['summer'] = "Summer" in data
            else:
                self.course['fall'] = "True"
                self.course['spring'] = "True"
                self.course['summer'] = "False"
        elif self.status == self.DESCRIPTION:
            if hasattr(self.course, "description"):
                self.course['description'] = self.course['description'] + data
            else:
                self.course['description'] = data
        elif self.status == self.PREREQS:
            self.course['prerequisites'] = data

    def get_data(self):
        return self.courses

course_url = "http://www2.acs.ncsu.edu/reg_records/crs_cat/%s.html"

prefix_file = open(os.path.join("..", "CourseData", "prefixes.txt"))
for line in prefix_file:
    prefix = (line.split("-")[0]).strip().upper()
    print(prefix)
    try:
        url_contents = urllib2.urlopen(course_url % prefix)
        parser = CourseParser()
        for html_line in url_contents:
            parser.feed(html_line)
        data = parser.get_data()
        json_file = open(os.path.join("..", "CourseData", "%s.json" % prefix), "w")
        json.dump(data, json_file)
        json_file.close()
    except urllib2.HTTPError as e:
        print("Could not load file")
    # exit(0)
