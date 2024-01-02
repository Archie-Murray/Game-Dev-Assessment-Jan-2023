import java.io.BufferedReader;
import java.io.FileReader;
import java.io.IOException;

/**
 * Class to find number of words in Implementation report
 */
class WordCount {

  /**
   * @return Logs word count to console
   */
  public static void main(String[] args) {
    String fileName = "./Implementation Report.md";
    int count = 0;
    try (BufferedReader fileReader = new BufferedReader(new FileReader(fileName))) {
      
      String line = "";
      boolean skip = false;
      do {
        line = fileReader.readLine();
        if (line == null) {
          break;
        }
        if (line.startsWith("```")) {
          skip = !skip;
        }

        if (skip) {
          continue;
        }
        count += line.split(" ").length;
      } while (true);
    } catch (IOException ioe) { }

    System.out.println("Words: " + count);
  }
}
