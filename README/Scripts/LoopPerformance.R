# Check if libraries are already loaded, otherwise load them
if (!require(ggplot2)) {
  install.packages("ggplot2")
  library(ggplot2)
}
if (!require(dplyr)) {
  install.packages("dplyr")
  library(dplyr)
}

# Read data from CSV file
df <- read.csv("Benchmarks.LoopingBenchmarks-report.csv", header = TRUE)  # Change to full path

# Clean up data with dplyr
data <- df %>% 
  mutate(Mean = as.numeric(gsub(",", "", gsub(" ns", "", Mean)))) %>% 
  select(Method, Capacity, Mean)


p <- ggplot(data, aes(x = Capacity, y = Mean)) + 
  geom_line(linetype = "dotted", size=0.75) + 
  geom_point() +
  facet_grid(. ~ Method, scales = "free") +
 # facet_wrap(~ Method, scales = "free") +
  scale_y_continuous(labels = function(y) format(y, big.mark = ".", decimal.mark = ",", scientific = FALSE)) +
  scale_x_continuous(labels = function(x) format(x, big.mark = ".", decimal.mark = ",", scientific = FALSE)) +
  theme_bw() +
  theme(plot.title = element_text(hjust = 0.5), legend.position = "none", axis.text.x = element_text(angle = 45, hjust = 1)) +
  labs(title = "Comparison of Methods by Capacity", x = "Capacity (number of integers)", y = "Mean (ms)") 

ggsave("side-by-side.png", p, width = 20, height = 10, dpi = 300)

p <- ggplot(data, aes(x = Capacity, y = Mean, color = Method)) + 
  geom_line(linetype = "dotted", size=0.75) + 
  geom_point() +
  scale_y_continuous(labels = function(y) format(y, big.mark = ".", decimal.mark = ",", scientific = FALSE)) +
  scale_x_continuous(labels = function(x) format(x, big.mark = ".", decimal.mark = ",", scientific = FALSE)) +
  scale_color_manual(values=c("black", "darkgreen", "darkred", "yellow", "darkblue", "lightblue", "violet")) +
  theme_bw() +
  theme(legend.position = "bottom") +
  labs(title = "Comparison of Methods by Capacity", x = "Capacity (number of integers)", y = "Mean (ns)") 


ggsave("all-in-one.png", p, width = 20, height = 10, dpi = 300)