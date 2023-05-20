using System.Data;

namespace CIE206PROJECT.Controllers
{
    public class CoursePage
    {
        private DB_Controller _Controller;

        public CoursePage()
        {
            _Controller = new DB_Controller();
        }

        public DataTable? getGroupsTrainer(int id)
        {
            string q = $@"""
                    SELECT g.group_no, c.course_name
                    FROM [group] g
                    JOIN offering o ON g.offering_id = o.offering_id
                    JOIN course c ON o.course_id = c.course_id
                    WHERE g.Trainer_id = {id};
            """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getGroupsStudent(int id)
        {
            string q = $@"""
                       SELECT [group].group_no, course.course_name, [user].[name] AS tutor_name
                    FROM Student_groups
                    JOIN [group] ON Student_groups.group_no = [group].group_no
                    JOIN offering ON [group].offering_id = offering.offering_id
                    JOIN course ON offering.course_id = course.course_id
                    JOIN Trainer ON [group].Trainer_id = Trainer.user_id
                    JOIN [user] ON Trainer.user_id = [user].user_id
                    WHERE Student_groups.Student_id = {id};
               """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getStudentInfo(int id)
        {
            string q = $@"""
                    SELECT Student.user_id,
                           [user].[name] AS student_name,
                           Student.skill_level,
                           [parent_user].[name] AS parent_name,
                           [user].email,
                           [user].[address],
                           [user].join_date,
                           [user].date_of_birth
                    FROM Student
                    JOIN [user] ON Student.user_id = [user].user_id
                    LEFT JOIN [user] AS [parent_user] ON Student.parent_id = [parent_user].user_id
                    WHERE Student.user_id = {id};                """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getUserPhonenumbers(int id)
        {
            string q = $@"""
                SELECT phone_num
                FROM [phone_num] 
                WHERE user_id={id};
                """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }


        public DataTable? getTrainerInfo(int id)
        {
            string q = $@"""
                SELECT *
                FROM Trainer 
                JOIN [user] ON Trainer.user_id = [user].user_id
                WHERE Trainer.user_id={id};
                """; 
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }
        public DataTable? getTrainerEvaluations(int id)
        {
            string q = $@"""
                        SELECT g.group_no,
                            ROUND(AVG(te.criteria_c1) * 100, 2) AS avg_criteria_c1_percentage,
                            ROUND(AVG(te.criteria_c2) * 100, 2) AS avg_criteria_c2_percentage,
                            ROUND(AVG(te.criteria_c3) * 100, 2) AS avg_criteria_c3_percentage,
                            ROUND(AVG(te.criteria_c4) * 100, 2) AS avg_criteria_c4_percentage
                        FROM [group] g
                        JOIN lecture l ON g.group_no = l.group_id
                        LEFT JOIN trainer_eval te ON l.lecture_id = te.lecture_id
                        WHERE l.trainer_id = {id}
                        GROUP BY g.group_no;         
                        """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getStudentEvaluations(int id)
        {
            string q = $@"""
                        SELECT g.group_no,
                            AVG(se.attendance) AS avg_attendance,
                            AVG(se.criteria_c1) AS avg_criteria_c1,
                            AVG(se.criteria_c2) AS avg_criteria_c2,
                            AVG(se.criteria_c3) AS avg_criteria_c3,
                            AVG(se.criteria_c4) AS avg_criteria_c4
                        FROM [group] g
                        JOIN lecture l ON g.group_no = l.group_id
                        LEFT JOIN student_eval se ON l.lecture_id = se.lecture_id
                        WHERE se.student_id = {id}
                        GROUP BY g.group_no;
            """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getGroupContent(int id)
        {
            string q = $@"""
                    SELECT
                      content.content_id,
                      content.summary,
                      content.summary_vid,
                      content.slides,
                      content.teacher_guide,
                      content.handout
                    FROM content
                    JOIN course ON content.course_id = course.course_id
                    JOIN offering ON course.course_id = offering.course_id
                    JOIN [group] ON offering.offering_id = [group].offering_id
                    WHERE [group].group_no ={id};
            """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getGroupContentTopics(int id)
        {
            string q = $@"""
                    SELECT g.group_no,
                        AVG(te.criteria_c1) AS avg_criteria_c1,
                        AVG(te.criteria_c2) AS avg_criteria_c2,
                        AVG(te.criteria_c3) AS avg_criteria_c3,
                        AVG(te.criteria_c4) AS avg_criteria_c4
                    FROM [group] g
                    JOIN lecture l ON g.group_no = l.group_id
                    LEFT JOIN trainer_eval te ON l.lecture_id = te.lecture_id
                    WHERE l.trainer_id = {id}
                    GROUP BY g.group_no;
            """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public int getStudentCount(int id){
            string q = $@"""
                        SELECT COUNT(Student_groups.Student_id) AS num_students
                        FROM Student_groups
                        WHERE Student_groups.group_no ={id};
                        """;
            int dt = (int)_Controller.Exec_Scalar(q);
            return dt;
 
        }

        public DataTable? getGroupInfo(int id)
        {
            string q = $@"""
                SELECT
                  course.course_name,
                  course.course_description,
                  course.tot_sessions,
                  offering.Start_Date,
                  offering.Price,
                  [group].group_no,
                  [group].Trainer_id,
                  [group].Timeslot,
                  [group].n_students,
                  [group].meeting_link,
                  [group].age_grp,
                  [user].[name] AS tutor_name,
                  [user].email AS tutor_email
                FROM [group]
                JOIN offering ON [group].offering_id = offering.offering_id
                JOIN course ON offering.course_id = course.course_id
                LEFT JOIN Trainer ON [group].Trainer_id = Trainer.user_id
                LEFT JOIN [user] ON Trainer.user_id = [user].user_id
                WHERE [group].group_no = {id};
            """;

            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getStudentsGroup(int id)
        {
            string q = $@"""
                JOIN [user] ON Student_gr
                SELECT [user].[name] AS student_name, [user].profile_pic 
                FROM Student_groupsoups.Student_id = [user].user_id
                WHERE Student_groups.group_no = {id};
            """;
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }

        public DataTable? getStudentsNotes(int id)
        {
            string q =$@"""
                SELECT r.subject, r.content, r.datetime AS date_sent, u.name AS sender_name, u.user_type AS sender_user_type
                FROM request r
                JOIN [user] u ON r.sent_by = u.user_id
                WHERE r.sent_to = {id};
           """;

            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }


        public bool createStudentNote()
        {
            string q = "";
            bool dt = _Controller.Exec_NonQ(q);

            return dt;
        }
        public bool deleteStudentNote(int id)
        {
            string q = $@"""
                    DELETE FROM request
                    WHERE sent_to = {id};
                    """;
            bool dt = _Controller.Exec_NonQ(q);
            return dt;
        }

        public DataTable? getUpcomingLecture(int id)
        {
            string q = $@"""SELECT *
            FROM lecture
            ORDER BY day DESC
            LIMIT 1;""";
            DataTable? dt = new DataTable();
            dt = _Controller.Exec_Queury(q);
            return dt;
        }
    }
}
