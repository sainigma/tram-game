public interface StateCollector {
    void report(int id, bool state);
    bool hasNext();
    ButtonState getNext();
}
